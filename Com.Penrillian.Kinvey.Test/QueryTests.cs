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
            var constraints = new KinveyConstraints<Giraffe>().Constrain(g => g.Name, Is.NotExistent<string>());
            var query = new KinveyQuery<Giraffe>(constraints);
            Assert.AreEqual("?query={\"name\":{\"$exists\":false}}", query.ToString());
        }

        [Test]
        public void AdoptionRulesPreferAdoptedConstraint()
        {
            var constraints = new KinveyConstraints<Giraffe>()
                                    .Constrain(g => g.Name, "steve")
                                    .Constrain(g => g.Age, 19);
            var query = new KinveyQuery<Giraffe>()
                                    .Constrain(g => g.Name, "dave");
            query.Adopt(constraints);
            Assert.AreEqual("?query={\"name\":\"steve\",\"age\":19}", query.ToString());
        }

        [Test]
        public void AdoptionRulesPreserveOrphanedConstraint()
        {
            var constraints = new KinveyConstraints<Giraffe>()
                                    .Constrain(g => g.Name, "steve");
            var query = new KinveyQuery<Giraffe>()
                                    .Constrain(g => g.Name, "dave")
                                    .Constrain(g => g.Age, 19);
            query.Adopt(constraints);
            Assert.AreEqual("?query={\"name\":\"steve\",\"age\":19}", query.ToString());
        }

        [Test]
        public void Or()
        {
            var constraints = new[]
                {
                    new KinveyConstraints<Giraffe>().Constrain(g => g.Name, "steve"), 
                    new KinveyConstraints<Giraffe>().Constrain(g => g.Name, "dave")
                };
            Assert.AreEqual("{\"$or\":[{\"name\":\"steve\"},{\"name\":\"dave\"}]}", constraints.Or().ToString());
        }

        [Test]
        public void Nor()
        {
            var constraints = new[]
                {
                    new KinveyConstraints<Giraffe>().Constrain(g => g.Name, "steve"), 
                    new KinveyConstraints<Giraffe>().Constrain(g => g.Name, "dave")
                };
            Assert.AreEqual("{\"$nor\":[{\"name\":\"steve\"},{\"name\":\"dave\"}]}", constraints.Nor().ToString());
        }

        [Test]
        public void And()
        {
            var constraints = new[]
                {
                    new KinveyConstraints<Giraffe>().Constrain(g => g.Name, "steve"), 
                    new KinveyConstraints<Giraffe>().Constrain(g => g.Age, 19)
                };
            Assert.AreEqual("{\"$and\":[{\"name\":\"steve\"},{\"age\":19}]}", constraints.And().ToString());
        }

        [Test]
        public void ComplexOrAnd()
        {
            var andConstraints1 = new[]
                {
                    new KinveyConstraints<Giraffe>().Constrain(g => g.Name, "steve"), 
                    new KinveyConstraints<Giraffe>().Constrain(g => g.Age, 19)
                }.And();
            var andConstraints2 = new[]
                {
                    new KinveyConstraints<Giraffe>().Constrain(g => g.Name, "dave"), 
                    new KinveyConstraints<Giraffe>().Constrain(g => g.Age, 20)
                }.And();
            var orConstraints = new[] { andConstraints1, andConstraints2 }.Or();
            Assert.AreEqual("{\"$or\":[{\"$and\":[{\"name\":\"steve\"},{\"age\":19}]},{\"$and\":[{\"name\":\"dave\"},{\"age\":20}]}]}", orConstraints.ToString());
        }

        [Test]
        public void ComplexAndOr()
        {
            var andConstraints1 = new[]
                {
                    new KinveyConstraints<Giraffe>().Constrain(g => g.Name, "steve"), 
                    new KinveyConstraints<Giraffe>().Constrain(g => g.Age, 19)
                }.Or();
            var andConstraints2 = new[]
                {
                    new KinveyConstraints<Giraffe>().Constrain(g => g.Name, "dave"), 
                    new KinveyConstraints<Giraffe>().Constrain(g => g.Age, 20)
                }.Or();
            var orConstraints = new[] { andConstraints1, andConstraints2 }.And();
            Assert.AreEqual("{\"$and\":[{\"$or\":[{\"name\":\"steve\"},{\"age\":19}]},{\"$or\":[{\"name\":\"dave\"},{\"age\":20}]}]}", orConstraints.ToString());
        }

        [Test]
        public void Not()
        {
            var constraints = new KinveyConstraints<Giraffe>().Constrain(g => g.Name, "steve").Not();
            Assert.AreEqual("{\"$not\":{\"name\":\"steve\"}}", constraints.ToString()); 
        }
    }
}
