using NUnit.Framework;

namespace Com.Penrillian.Kinvey.Test
{
    [TestFixture]
    public class QueryTests
    {
        [Test]
        public void Limit()
        {
            var query = new KinveyQuery<Giraffe>().Limit(10);
            Assert.AreEqual("?query={}&limit=10", query.ToString());
        }

        [Test]
        public void Skip()
        {
            var query = new KinveyQuery<Giraffe>().Skip(10);
            Assert.AreEqual("?query={}&skip=10", query.ToString());
        }

        [Test]
        public void LimitSkip()
        {
            var query = new KinveyQuery<Giraffe>().Limit(5).Skip(10);
            Assert.AreEqual("?query={}&limit=5&skip=10", query.ToString());
        }

        [Test]
        public void Constraint()
        {
            var query = new KinveyQuery<Giraffe>().Constrain(g => g.Name, "steve");
            Assert.AreEqual("?query={\"name\":\"steve\"}", query.ToString());
        }

        [Test]
        public void LimitConstraint()
        {
            var query = new KinveyQuery<Giraffe>().Limit(10).Constrain(g => g.Name, "steve");
            Assert.AreEqual("?query={\"name\":\"steve\"}&limit=10", query.ToString());
        }

        [Test]
        public void SkipConstraint()
        {
            var query = new KinveyQuery<Giraffe>().Skip(10).Constrain(g => g.Name, "steve");
            Assert.AreEqual("?query={\"name\":\"steve\"}&skip=10", query.ToString());
        }

        [Test]
        public void LimitSkipConstraint()
        {
            var query = new KinveyQuery<Giraffe>().Limit(5).Skip(10).Constrain(g => g.Name, "steve");
            Assert.AreEqual("?query={\"name\":\"steve\"}&limit=5&skip=10", query.ToString());
        }

        [Test]
        public void GreaterThanConstraint()
        {
            var query = new KinveyQuery<Giraffe>().Constrain(g => g.Name, Is.GreaterThan("steve"));
            Assert.AreEqual("?query={\"name\":{\"$gt\":\"steve\"}}", query.ToString());
        }

        [Test]
        public void LessThanConstraint()
        {
            var query = new KinveyQuery<Giraffe>().Constrain(g => g.Name, Is.LessThan("steve"));
            Assert.AreEqual("?query={\"name\":{\"$lt\":\"steve\"}}", query.ToString());
        }

        [Test]
        public void GreaterThanEqualToConstraint()
        {
            var query = new KinveyQuery<Giraffe>().Constrain(g => g.Name, Is.GreaterThanEqualTo("steve"));
            Assert.AreEqual("?query={\"name\":{\"$gte\":\"steve\"}}", query.ToString());
        }

        [Test]
        public void LessThanEqualToConstraint()
        {
            var query = new KinveyQuery<Giraffe>().Constrain(g => g.Name, Is.LessThanEqualTo("steve"));
            Assert.AreEqual("?query={\"name\":{\"$lte\":\"steve\"}}", query.ToString());
        }

        [Test]
        public void GreaterThanLessThanConstraint()
        {
            var query = new KinveyQuery<Giraffe>().Constrain(g => g.Name, Is.GreaterThan("steve").LessThan("dave"));
            Assert.AreEqual("?query={\"name\":{\"$gt\":\"steve\",\"$lt\":\"dave\"}}", query.ToString());
        }

        [Test]
        public void InConstraint()
        {
            var query = new KinveyQuery<Giraffe>().Constrain(g => g.Name, Is.In(new[] { "steve", "dave" }));
            Assert.AreEqual("?query={\"name\":{\"$in\":[\"steve\",\"dave\"]}}", query.ToString());
        }

        [Test]
        public void NeConstraint()
        {
            var query = new KinveyQuery<Giraffe>().Constrain(g => g.Name, Is.NotEqualTo("steve"));
            Assert.AreEqual("?query={\"name\":{\"$ne\":\"steve\"}}", query.ToString()); 
        }

        [Test]
        public void NotInConstraint()
        {
            var query = new KinveyQuery<Giraffe>().Constrain(g => g.Name, Is.NotIn(new[] { "steve", "dave" }));
            Assert.AreEqual("?query={\"name\":{\"$nin\":[\"steve\",\"dave\"]}}", query.ToString());
        }

        [Test]
        public void ExistentConstraint()
        {
            var query = new KinveyQuery<Giraffe>().Constrain(g => g.Name, Is.Existent<string>());
            Assert.AreEqual("?query={\"name\":{\"$exists\":true}}", query.ToString());
        }

        [Test]
        public void NotExistentConstraint()
        {
            var query = new KinveyQuery<Giraffe>().Constrain(g => g.Name, Is.NotExistent<string>());
            Assert.AreEqual("?query={\"name\":{\"$exists\":false}}", query.ToString());
        }
    }
}
