namespace zenonApi.Serialization
{
  public interface IZenonSerializable<out TSelf, out TParent, out TRoot>
    : IZenonSerializable<TSelf>
    where TSelf : class, IZenonSerializable<TSelf>
  {
    TParent Parent { get; }
    TRoot Root { get; }
  }

  public interface IZenonSerializable<out TSelf> : IZenonSerializable
    where TSelf : class
  { }

  public interface IZenonSerializable
  {
    string NodeName { get; }
  }
}
