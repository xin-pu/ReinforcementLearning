﻿using DeepSharp.RL.Agents;
using DeepSharp.RL.Environs;

namespace TorchSharpTest.RLTest
{
    public class QLearningTest : AbstractTest
    {
        public QLearningTest(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
        }


        [Fact]
        public void KArmedBanditMain()
        {
            /// Step 1 Create a 4-Armed Bandit
            var kArmedBandit = new KArmedBandit(new[] {0.4, 0.8, 0.3, 0.75}) {Gamma = 0.95f};

            /// Step 2 Create AgentCrossEntropy with 0.7f percentElite as default
            var agent = new QLearning(kArmedBandit);
            Print(kArmedBandit);

            var i = 0;
            var testEpisode = 20;
            var bestReward = 0f;
            while (true)
            {
                i++;
                kArmedBandit.Reset();
                var epoch = 0;
                while (!kArmedBandit.IsComplete(epoch))
                {
                    var action = agent.GetEpsilonAct(kArmedBandit.Observation!.Value!);
                    var step = kArmedBandit.Step(action, epoch++);
                    agent.Update(step);
                }


                var episode = agent.RunEpisode(testEpisode);
                var reward = episode.Average(a => a.SumReward.Value);

                bestReward = new[] {bestReward, reward}.Max();
                Print($"{agent} Play:{i:D3}\t {reward}");
                if (bestReward > 16)
                    break;
            }

            var e = agent.RunEpisode();
            var act = e.Steps.Select(a => a.Action);
            Print(string.Join("\r\n", act));
        }


        [Fact]
        public void FrozenlakeMain()
        {
            /// Step 1 Create a 4-Armed Bandit
            var frozenlake = new Frozenlake(deviceType: DeviceType.CPU) {Gamma = 0.95f};

            /// Step 2 Create AgentCrossEntropy with 0.7f percentElite as default
            var agent = new QLearning(frozenlake);
            Print(frozenlake);

            var i = 0;
            var testEpisode = 20;
            var bestReward = 0f;

            while (true)
            {
                frozenlake.Reset();
                var epoch = 0;
                while (!frozenlake.IsComplete(epoch))
                {
                    var action = agent.GetEpsilonAct(frozenlake.Observation!.Value!);
                    var step = frozenlake.Step(action, epoch++);
                    agent.Update(step);
                }


                var episode = agent.RunEpisode(testEpisode);
                var reward = episode.Average(a => a.SumReward.Value);

                bestReward = new[] {bestReward, reward}.Max();
                Print($"{agent} Play:{i:D3}\t {reward}");
                if (bestReward > 0.8)
                    break;
            }

            var e = agent.RunEpisode();
            var act = e.Steps.Select(a => a.Action);
            Print(string.Join("\r\n", act));
        }
    }
}