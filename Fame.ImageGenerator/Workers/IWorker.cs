using System;
using System.Threading.Tasks;

namespace Fame.ImageGenerator.Workers
{
	public interface IWorker<T>
	{
		Task<Object> Process(T e);
	}
}
