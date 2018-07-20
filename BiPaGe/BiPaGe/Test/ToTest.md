# Stuff that we need to add to unit tests

## Semantic Analysis
- Check if expressions in size field always return a positive number (if they can be resolved at compile time that is)
- Enumerators must have a value (e.g. `a: uint8 { aap, noot, mies }` is not supported)
- Enumeration types must be integral
- Anonimous structures and enumerations must have unique names. For now this means that anonious structure and enumeration fields must have unique names. We could add some elaborate code to create a unique name even when that's not the case, but I don't feel that the feature is worth the effort. 
If you really need two fields of the same type with the same name in your data structures, just don't use anonimous types so you can explicitly name them. 
