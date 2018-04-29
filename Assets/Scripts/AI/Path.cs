// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
// Path.cs. 
// 
// Vpetrov. Петров Василий Александрович. 
// 
// 2017
namespace Assets.Scripts
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	
	public class Path : ICloneable
	{
		public List<PathStep> Steps { get; set; }

		public float Length
		{
			get
			{
				return GetLength();
			}
		}

		public int StepsCount
		{
			get
			{
				return Steps.Count;
			}
		}

		public Path()
		{
			Steps = new List<PathStep>();
		}

		public void Add(PathStep step)
		{
			Steps.Add(step);
		}

		public PathStep this[int index]
		{
			get
			{
				return Steps[index];
			}
		}

		private float GetLength()
		{
			return Steps.Sum(s => s.StepLength);
		}

		public object Clone()
		{
			var clone = new Path();
			var steps = new PathStep[Steps.Count];
			Steps.CopyTo(steps);
			clone.Steps = steps.ToList();

			return clone;
		}
	}
}