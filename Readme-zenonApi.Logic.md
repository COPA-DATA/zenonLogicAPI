# zenonApi.Logic

Please note: This API is work in progress and might change significantly in the future.
If you encounter any bugs, please give us feedback.

## Next major steps
- [ ] Refactor the serializer:
  - [ ] The NodeName property was redundant and was removed
  - [ ] Dictionaries can be serialized
  - [ ] Support Converters for zenonSerializableNodeContent
  - [ ] Support Converters for zenonSerializableNodes
  - [ ] Do we really need the "Parent" property everywhere?
        Shouldn't we use Parents only for file structures etc?
- [ ] Map the K5Project tree completely
- [ ] Copy the entire documentation from the K5Help.xml to documentation tags in the source code
- [ ] Ensure readonly properties have no public setter
- [ ] Ensure classes which are not required by users are hidden
- [ ] Ensure that no invalid objects can be created, i.e. check constructor parameters
- [ ] Downgrade .NET version to lowest possible .NET standard version for hightest compatibility
- [ ] Provide samples for Add-In/COM