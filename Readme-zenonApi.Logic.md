# zenonApi.Logic

## Long-Term Roadmap
- [ ] Map the K5Project tree completely
- [ ] Copy the entire documentation from the K5Help.xml to documentation tags in
      the source code
- [ ] Implement custom lists, which are "parent-aware", to allow methods like
      ```Remove()``` on e.g. a ```LogicFolder``` and to handle logic for
      removal, adding, etc. automatically
- [ ] Ensure readonly properties have no public setter
- [ ] Ensure classes which are not required by users are hidden
- [ ] Ensure that no invalid objects can be created, i.e. check constructor
      parameters