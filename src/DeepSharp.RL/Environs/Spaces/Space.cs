﻿using FluentAssertions;

namespace DeepSharp.RL.Environs.Spaces
{
    public abstract class Space : IDisposable
    {
        protected Space(
            long[] shape,
            torch.ScalarType type,
            DeviceType deviceType = DeviceType.CUDA,
            long seed = 1)
        {
            CheckInitParameter(shape, type);
            (Shape, Type, DeviceType) = (shape, type, deviceType);
            Generator = torch.random.manual_seed(seed);
        }

        public long[] Shape { get; }
        public torch.ScalarType Type { get; }
        public DeviceType DeviceType { get; }
        internal torch.Generator Generator { get; }

        public void Dispose()
        {
            Generator.Dispose();
        }

        /// <summary>
        ///     Returns a sample from the space.
        /// </summary>
        /// <returns></returns>
        public abstract torch.Tensor Sample();

        /// <summary>
        ///     Generates a tensor whose shape and type are consistent with the space definition.
        /// </summary>
        /// <returns></returns>
        public virtual torch.Tensor Generate()
        {
            return torch.zeros(Shape, Type, new torch.Device(DeviceType));
        }

        public override string ToString()
        {
            return $"Space Type: {GetType().Name}\nShape: {Shape}\ndType: {Type}";
        }

        private static void CheckInitParameter(long[] shape, torch.ScalarType type)
        {
            shape.Should().NotBeNull();
            shape.Length.Should().BeGreaterThan(0);
        }
    }
}