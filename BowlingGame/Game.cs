using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace BowlingGame
{
    public class Frame
    {
        private static int IdCounter = 0;
        public int Id { get; set; }
        public int FirstRoll { get; set; }
        public int SecondRoll { get; set; }
        public bool IsStrike;
        public bool IsSpare;
        public int Score { get; set; }
        public bool NeedUpdate;

        public Frame(int firstRoll, int secondRoll)
        {
            FirstRoll = firstRoll;
            SecondRoll = secondRoll;
            Id = IdCounter++;
            Score = FirstRoll + SecondRoll;

            IsStrike = FirstRoll == 10;
            IsSpare = FirstRoll + SecondRoll == 10 && !IsStrike;
            NeedUpdate = IsStrike || IsSpare;
        }
    }

    public class Game
	{
	    public readonly List<Frame> frames = new List<Frame>();
	    public int curruntScore { get; private set; }

	    public Frame PlayFrame(int pins1, int pins2)
	    {
            Roll(pins1);
            Roll(pins2);
	        return new Frame(pins1, pins2);
	    }

	    public void UpdateFrame(int frameId)
	    {
	        var frame = frames[frameId];
            if(!frame.NeedUpdate)
                return;
	        if (frame.IsStrike)
	        {
	            if (frames.Count > frameId + 1 && !frames[frameId + 1].IsStrike)
	            {
	                frame.Score += frames[frameId + 1].Score;
	                frame.NeedUpdate = false;
                    return;
	            }
                if (frames.Count > frameId + 2 && frames[frameId + 1].IsStrike)
                {
                    frame.Score += frames[frameId + 1].Score + frames[frameId + 2].FirstRoll;
                    frame.NeedUpdate = false;
                    return;
                }
	        }
	    }

	    public void Log(Frame fr)
	    {
	        frames.Add(fr);
	        curruntScore = 0;
	    }

		public void Roll(int pins)
		{
		    curruntScore += pins;
		}

		public int GetScore()
		{
		    return frames.Select(e => e.Score).Sum();
		}
	}


	[TestFixture]
	public class Game_should : ReportingTest<Game_should>
	{
		// ReSharper disable once UnusedMember.Global
		public static string Names = "7 Soldatenko Mikheev"; // Ivanov Petrov

	    private Game game;


	    [SetUp]
	    public void SetUp()
	    {
	        game = new Game();
	    }

		[Test]
		public void HaveZeroScore_BeforeAnyRolls()
		{
			new Game()
				.GetScore()
				.Should().Be(0);
		}

        [Test]
        public void GetScore_Should_CorrectScore_AfterOneRoll()
	    {
	        game.Roll(7);
	        game.curruntScore.Should().Be(7);
	    }

	    [Test]
	    public void PlayFrame_Should_ReturnCorrectFrame()
	    {
	        var expectedFrame = new Frame(7, 2);
            game.PlayFrame(7, 2).Score.Should().Be(expectedFrame.Score);
	    }

	    [Test]
	    public void Frame_IsStrike()
	    {
	        game.PlayFrame(10, 0).IsStrike.Should().BeTrue();
	    }

	    [Test]
	    public void Frame_IsSpare()
	    {
	        game.PlayFrame(6, 4).IsSpare.Should().BeTrue();
	    }

	    [Test]
	    public void GetScore_Should_RetrunCorrectScoreWithoutSpareAndStrike()
	    {
	        game.Log(game.PlayFrame(2, 4));
	        game.Log(game.PlayFrame(5, 2));
	        game.GetScore().Should().Be(13);
	    }

        [Test]
        public void SpareFrame_HaveCorrectScoreWithSpare()
        {
            game.Log(game.PlayFrame(7, 3));
            game.Log(game.PlayFrame(5, 1));
            game.frames[0].Score.Should().Be(15);
        }
    }
}
