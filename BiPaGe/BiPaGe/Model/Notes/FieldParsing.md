#Field parsing

All fields have the following properties
- a type (int32, int12, enumeration, complex type, etc)
- An offset **in bits**
- A size **in bits**
- A 'from': the name of the dynamic field from which the offset is valid

##Type
The type of a field in the DSL is not necessarilly the same as the field type in the target language. For C++ for instance, no such thing as a 5 bit intgeger exists (I suppose this is true for most if not all programming languages, but in theory, this is a language dependent construct.)
So we need to convert it to the type that fits best. In the case of the 6 bit intgeger, this would be an 8 bit integer (or, one could argue, just an integer, use the size most suitable for the platform as long as it is not too short)

##Offset in bits
We need to specify the offset in bits since we support 'weird sized types'. See the following example for the offsets
SomeObject {
	f1 : u6;		// offset: 0
	f2 : bool;		// offset: 6
	f3 : bool;		// offset: 7
	f4 : u32[5];	// offset: 8
	f5 : u4;		// offset 168
	f6 : s4;		// offset 172
}

##size in bits
Pretty straightforward for simple types (e.g. u6), not so straightforward for dynamic types (e.g some_collection : u8[some_other_field])

##From
From is used for fields that come after a dynamic object. For instance, behold the following object:
SomeObject {
	f1 : u6;		// offset: 0
	f2 : bool;		// offset: 6
	f3 : bool;		// offset: 7
	f4 : u32[f1];	// offset: 8
	f5 : u4;		// offset ?
	f6 : s4;		// offset ?
}

We can't supply a static offset from the start of the object for fields f5 and f6. We can however give the static offset from the end of f4:

SomeObject {
	f1 : u6;		// offset: 0, from 'start'
	f2 : bool;		// offset: 6, from 'start'
	f3 : bool;		// offset: 7, from 'start'
	f4 : u32[f1];	// offset: 8, from 'start'
	f5 : u4;		// offset 0, from f4
	f6 : s4;		// offset 4, from f4
}

## C++ specific

### Determine the best encapsulating type
Let's say we need to decode an u16 that has an offset of 35 bits. I'll try to draw it
```
[ | | | | | | | ][ | | | | | | | ][ | | | | | | | ][ | | | | | | | ][ | | |*|*|*|*|*][*|*|*|*|*|*|*|*][*|*|*| | | | | ]
```
To expose the data, we need to do 3 things:
In order to return data of weird sizes, there's a couple of things we need to do:
1. Capture the data as a supported data type
2. Mask out all the bits that don't belong to the data
3. Shift the masked data into the right position

For the first step, we need to find a data type that contains all the bits that belong to the data, starts on a byte boundary (because we can only create pointers to bytes), _and_ is a valid cpp type (in practice this means that the size of the type needs to be a power of 2 and 8 or greater)
In this example, that means that we need a 32 bit type:
```
[ | | | | | | | ][ | | | | | | | ][ | | | | | | | ][ | | | | | | | ][ | | |*|*|*|*|*][*|*|*|*|*|*|*|*][*|*|*| | | | | ][ | | | | | | | ]
                                                                    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ 
```
Now we have determined the width of the capture type (32 bits), but we also need to determine the offset to the capture type in order to be able to access it.
The rules here are relatively simple:
The offset is the first byte boundary _before_ the start of the data. In other words, the offset minus the offset modulo 8: offset - (offset % 8). Which in this case means 35 - 3 = 32
Now that we know the offset, we know that we have to capture at least the data size, plus the distance from the byte algined offset to the data offset. In this case (16 + 3) 19. Since c++ data (integral) types all have a width that is a power of 2 (with a minimum width of 8), we can define the 
width of the capture type as follows
minimum_capture = data_size + (data_offset - aligned_offset)
capture_size = min(2^(Ceil(2Log(minimum_capture)), 8)
Basically we determine which power of 2 the minimum size is, round that up and use that as a power of 2 to get to the proper capture size. We then make sure not to use anything smaller than 8.

In our example this would be
```
max(2^(Ceil(2Log(16+(35-32)))),8)
max(2^(Ceil(2Log(19))),8)
max(2^(Ceil(~4.248)),)
max(2^5,8)
32
```

So we need a 32 bit type to capture the data. Which is what we found when looking at the data as well, so that's good.

The next step is to determine the mask that we need to use to black out all of the data that got into the capture type but isn't part of the data type we want to 'parse'.
That's pretty easy. We know the data type's size in bits, so we can simply start off with `mask = 2(^<data_size>) -1`. This mask is the right size, but we need to shift it to mask the right data:
`mask = mask << capture_size - data_size - (data_offset - aligned_offset)`

In our example, this would boil down to
```
mask = (2^16) -1
mask = mask << (32 - 16 - (35-32))
```
or
```
mask = (0xffff << 13)
```

The last step is to shift the masked data out to the right. After the masking process, we have effectively set all bits that we don't care about to 0:
```
[0|0|0|*|*|*|*|*][*|*|*|*|*|*|*|*][*|*|*|0|0|0|0|0][0|0|0|0|0|0|0|0]
```
But in order to make use of the type, we need to shift out those zeroes that are on the right of the actual data. This too is pretty easy: because we are shifting the masked data right the same amount of bits that we shifted the mask left:
```
data = data >> (capture_size - data_size - (data_offset - aligned_offset))
```

Or, to put each step visually visually:
The original data:
```
[ | | | | | | | ][ | | | | | | | ][ | | | | | | | ][ | | | | | | | ][ | | |*|*|*|*|*][*|*|*|*|*|*|*|*][*|*|*| | | | | ]
```
The captured data:
```
[ | | |*|*|*|*|*][*|*|*|*|*|*|*|*][*|*|*| | | | | ][ | | | | | | | ]
```
The initial mask:
```
[ | | |*|*|*|*|*][*|*|*|*|*|*|*|*][*|*|*| | | | | ][ | | | | | | | ]
                                   ^ ^ ^ ^ ^ ^ ^ ^  ^ ^ ^ ^ ^ ^ ^ ^
```
The mask after shifting 13 places to the left:
```
[ | | |*|*|*|*|*][*|*|*|*|*|*|*|*][*|*|*| | | | | ][ | | | | | | | ]
       ^ ^ ^ ^ ^ ^ ^ ^  ^ ^ ^ ^ ^  ^ ^ ^
```
The masked data:
```
[0|0|0|*|*|*|*|*][*|*|*|*|*|*|*|*][*|*|*|0|0|0|0|0][0|0|0|0|0|0|0|0]
```
The masked data after shifting it right:
The masked data:
```
[0|0|0|0|0|0|0|0][0|0|0|0|0|0|0|0][*|*|*|*|*|*|*|*][*|*|*|*|*|*|*|*]
```
   