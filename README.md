# zenonApi packages

Please note: This API is work in progress and might change significantly in the future.
If you encounter any bugs, please give us feedback.

Documentation is currently sparse and needs to be added in the future.
See the sample project on how to use the API.

## Changelog

### zenonApi.Core

#### 1.1.x.x

- Made NodeName optional for deriving classes
- Added Resolver support for varying nodes and abstract/interface properties
- Added OnDeserialized/OnSerialize/OnSerialized to zenonSerializable

#### 1.2.x.x

- Added support for Nullable&lt;T&gt; serialization/deserialization without extra converters
- Added support for Enums in nodes
- Bugfix for zenonSerializableNodeContent: Was possible that the full node was overriden by it
- Added UnknownNodeContent
- Set default encoding for logic-projects to iso-8859-1 (otherwise we had formatting issues during import, logic ignores the actual xml encoding)
- Added further Import/Export methods for streams/files/etc.
- Fixes for serializing lists/arrays of primitive types

## Next major steps

- [ ] Refactor the serializer:  
  - [ ] Implement unit tests before refactoring:  
    This project came from a POC and grew too quickly without unit tests.  
    Although the code needs cleanup, it is already used for several projects, so unit tests are crucial.
  - [ ] Actual refactoring
  - [ ] Improve performance (as already mentioned, I created the Core API as a POC during a weekend)
- [ ] Dictionaries to be serialized
- [ ] Support Converters for zenonSerializableNodeContent
- [ ] Support Converters for zenonSerializableNodes
- [x] Support for UnknownNodeContent (added in a later build of v1.2)
- [ ] Do we really need the "Parent" property everywhere?  
      Shouldn't we use Parents only for file structures etc?  
- [ ] Map the K5Project tree completely
- [ ] Ensure all default K5 types are contained in a new logic project
- [ ] Copy the entire documentation from the K5Help.xml to documentation tags in the source code
- [ ] Ensure readonly properties (of the logic object model) have no public setter
- [ ] Ensure classes which are not required by users are hidden
- [ ] Ensure that no invalid objects can be created, i.e. check constructor parameters in the logic API
- [ ] Downgrade .NET version to lowest possible .NET standard version for highest compatibility
- [x] Provide samples for Add-In/COM
- [x] Support for Nullable<T> serialization and deserialization (added in v1.2)

## Bugs

- [ ] zenonApi.Core
  - [ ] On hold: Up to current version: OmitIfNull in zenonSerializableAttributes does not seem to be considered.
  - [x] Major: zenonSerializableNodeContent may override the full node if present (fixed in v1.2)