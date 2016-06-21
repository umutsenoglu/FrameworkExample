using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;

namespace Common.Validation.Tests
{

    [TestClass()]
    public class DataAnnotionValidatorTests
    {
        DataAnnotionValidator validator;
        public DataAnnotionValidatorTests()
        {
            validator = new DataAnnotionValidator();
        }

        [TestMethod()]
        public void validated_test()
        {
            var employe = new Employe() { Id = 1, Name = "Murat" };
            Assert.IsTrue(validator.Validate(employe));
        }
        [TestMethod()]
        public void unvalidated_test()
        {
            var employe = new Employe() { Id = 1, Name = "İsa" };
            Assert.IsFalse(validator.Validate(employe));
        }
    }
    public class Employe
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 5)]
        public string Name { get; set; }

    }
}