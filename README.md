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

- Added support for Nullable<T> serialization/deserialization without extra converters
- Added support for Enums in nodes
- Bugfix for zenonSerializableNodeContent: Was possible that the full node was overriden by it
- Added UnknownNodeContent

## Next major steps

- [ ] Refactor the serializer:
  - [ ] Dictionaries to be serialized
  - [ ] Support Converters for zenonSerializableNodeContent
  - [ ] Support Converters for zenonSerializableNodes
  - [ ] Support for UnknownNodeContent
  - [ ] Do we really need the "Parent" property everywhere?
        Shouldn't we use Parents only for file structures etc?
- [ ] Map the K5Project tree completely
- [ ] Copy the entire documentation from the K5Help.xml to documentation tags in the source code
- [ ] Ensure readonly properties have no public setter
- [ ] Ensure classes which are not required by users are hidden
- [ ] Ensure that no invalid objects can be created, i.e. check constructor parameters
- [ ] Downgrade .NET version to lowest possible .NET standard version for hightest compatibility
- [ ] Provide samples for Add-In/COM
- [x] Support for Nullable<T> serialization and deserialization (added in v1.2)

## Bugs

- [ ] zenonApi.Core
  - [ ] Minor: Up to current version: OmitIfNull in zenonSerializableAttributes does not seem to be considered.
  - [x] Major: zenonSerializableNodeContent may override the full node if present (fixed in v1.2)