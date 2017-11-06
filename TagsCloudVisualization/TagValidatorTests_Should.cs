using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class TagValidatorTests_Should
    {
        [SetUp]
        public void SetUp()
        {
            validator = new TagValidator(800, 600);
        }

        private TagValidator validator;

        [TestCase(-1, 0, 100, 100, TestName = "when location x-coordinat is less than zero")]
        [TestCase(0, -1, 100, 100, TestName = "when location y-coordinat is less than zero")]
        [TestCase(-1, -1, 100, 100, TestName = "when both location coordinats are less than zero")]
        [TestCase(900, 0, 10, 100, TestName = "when locationX is greater than image width")]
        [TestCase(500, 700, 200, 100, TestName = "when locationY is greater than image height")]
        [TestCase(500, 0, 400, 100, TestName = "when locationX + rctangleWidth is greater than image width")]
        [TestCase(0, 500, 100, 400, TestName = "when locationY + rctangleHeight is greater than image heigth")]
        public void RectangleIsCorrect_Should_BeFalse(
            int locationX, int locationY, int rectangleWidht, int rectangleHeight)
        {
            validator.RectangleIsCorrect(new Rectangle(
                new Point(locationX, locationY), new Size(rectangleWidht, rectangleHeight))).Should().BeFalse();
        }

        [TestCase(0, 0, 100, 100, TestName = "when location rectangle parametrs are correct")]
        public void RectangleIsCorrect_Should_BeTrue(
            int locationX, int locationY, int rectangleWidht, int rectangleHeight)
        {
            validator.RectangleIsCorrect(new Rectangle(
                new Point(locationX, locationY), new Size(rectangleWidht, rectangleHeight))).Should().BeTrue();
        }

        [TestCase(0, 0, TestName = "when both a width and height are equal zero")]
        [TestCase(-1, 0, TestName = "when width is less than zero")]
        [TestCase(0, -1, TestName = "when height is less than zero")]
        [TestCase(900, 0, TestName = "when width is greater than image width")]
        [TestCase(10, 700, TestName = "when height is greater than image height")]
        public void SizeIsCorrect_Should_BeFalse(int width, int height)
        {
            validator.SizeIsCorrect(new Size(width, height));
        }

        [TestCase(50, 60, TestName = "when both a width and height are corect")]
        public void SizeIsCorrect_Should_BeTrue(int width, int height)
        {
            validator.SizeIsCorrect(new Size(width, height));
        }
    }
}