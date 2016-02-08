namespace Bone.Generate
{
    public class Gen {
        static public T Generate<T>() where T : new() {
            return new T();
        }   
    }
}