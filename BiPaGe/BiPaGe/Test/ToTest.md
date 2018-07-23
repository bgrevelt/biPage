# Stuff that we need to add to unit tests

## Semantic Analysis
- Check if expressions in size field always return a positive number (if they can be resolved at compile time that is)
- Enumerators must have a value (e.g. `a: uint8 { aap, noot, mies }` is not supported)
- Enumeration types must be integral
- Anonimous structures and enumerations must have unique names. For now this means that anonious structure and enumeration fields must have unique names. We could add some elaborate code to create a unique name even when that's not the case, but I don't feel that the feature is worth the effort. 
If you really need two fields of the same type with the same name in your data structures, just don't use anonimous types so you can explicitly name them. 
- Should the bigger fields be byte aligned? In order to keep things (relatively) simple we don't want the encapsulating type to get bigger then 64 bits because we want simply create a pointer to the encapsulating type.
We can catch this in the SA and possibly add support for larger, non aligned types later. However, I don't think this will be a problem in practive since the larger fields are typically byte aligned.