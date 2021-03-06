// Copyright 2007-2008 The Apache Software Foundation.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace Magnum.Fibers
{
	using System;

	public interface Fiber
	{
		/// <summary>
		/// Enqueue a single action to the queue
		/// </summary>
		/// <param name="action"></param>
		void Add(Action action);

		/// <summary>
		/// Enqueue a series of actions to the queue
		/// </summary>
		/// <param name="actions"></param>
		void AddMany(params Action[] actions);

		/// <summary>
		/// Runs all remaining actions, waiting until all actions have been executed or until the
		/// timeout expires. If the timeout expires, an exception is thrown.
		/// </summary>
		/// <param name="timeout">The time to wait for all pending actions to be executed before throwing an exception</param>
		void Shutdown(TimeSpan timeout);

		/// <summary>
		/// Stops the fiber, discards any remaining actions, and prevents new actions from being added
		/// </summary>
		void Stop();
	}
}