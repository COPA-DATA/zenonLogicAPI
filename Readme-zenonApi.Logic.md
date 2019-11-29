# zenonApi.Logic

## Short-Term Roadmap
- [ ] Refactor the serializer:
  - [ ] The NodeName property was redundant and was removed
  - [ ] Dictionaries can be serialized
  - [ ] Support Converters for zenonSerializableNodeContent
  - [ ] Support Converters for zenonSerializableNodes
  - [ ] Do we really need the "Parent" property everywhere?
        Shouldn't we use Parents only for file structures etc?

## Long-Term Roadmap
- [ ] Map the K5Project tree completely
- [ ] Copy the entire documentation from the K5Help.xml to documentation tags in
      the source code
- [x] Implement custom lists, which are "parent-aware", to allow methods like
      ```Remove()``` on e.g. a ```LogicFolder``` and to handle logic for
      removal, adding, etc. automatically
- [ ] Ensure readonly properties have no public setter
- [ ] Ensure classes which are not required by users are hidden
- [ ] Ensure that no invalid objects can be created, i.e. check constructor
      parameters
- [ ] Downgrade .NET version to lowest possible .NET standard version for hightest
      compatibility