# Stuff that we need to add to unit tests

## Semantic Analysis
- Check if expressions in size field always return a positive number (if they can be resolved at compile time that is)
- Enumerators must have a value (e.g. `a: uint8 { aap, noot, mies }` is not supported)
- Enumeration types must be integral