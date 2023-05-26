﻿using RL.Core.Policies;

namespace RL.Core
{
    /// <summary>
    ///     智能体
    /// </summary>
    public abstract class Agent : ObservableObject
    {
        private IPolicy? _policy;
        private Reward[] _rewards = Array.Empty<Reward>();

        public Reward[] Rewards
        {
            set => SetProperty(ref _rewards, value);
            get => _rewards;
        }

        public IPolicy? Policy
        {
            set => SetProperty(ref _policy, value);
            get => _policy;
        }

        /// <summary>
        ///     根据rewards 学习或者更新策略
        /// </summary>
        public abstract IPolicy LearnPolicy(Reward[] rewards);


        /// <summary>
        ///     策略函数，根据最新状态，生成新的动作
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public abstract torch.Tensor RunPolicy(State state);
    }
}