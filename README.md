LinqScribe is a lightweight C# library that enables expressive, dynamic, and type-safe filtering of IQueryable<T> collections based on structured filter objects. 
It automatically maps filter properties to entity fields and constructs the corresponding LINQ expressions at runtime; eliminating repetitive boilerplate and simplifying API query logic.

### Key Features
- Auto-maps filters: Match filter object properties to entity fields by name (IN PROGRESS).
- Pluggable and extensible: Add support for ranges, strings (Contains, StartsWith), enums, and more (NOT STARTED).
- EF Core-ready: Compatible with IQueryable sources from Entity Framework.
