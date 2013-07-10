using Newtonsoft.Json;

namespace Com.Penrillian.Kinvey.Test
{
    [KinveyCollection("giraffe")]
    public class Giraffe : KinveyObject
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "age")]
        public int Age { get; set; }

        private bool Equals(KinveyObject other)
        {
            return string.Equals(Id, other.Id);
        }

        public override bool Equals(object obj)
        {
            // ReSharper disable ConvertIfStatementToReturnStatement
            // readability
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Giraffe)obj);
            // ReSharper restore ConvertIfStatementToReturnStatement
        }

        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }
    }

    public class UnmarkedGiraffe : KinveyObject
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "age")]
        public int Age { get; set; }

        private bool Equals(KinveyObject other)
        {
            return string.Equals(Id, other.Id);
        }

        public override bool Equals(object obj)
        {
            // ReSharper disable ConvertIfStatementToReturnStatement
            // readability
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Giraffe)obj);
            // ReSharper restore ConvertIfStatementToReturnStatement
        }

        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }
    }
}