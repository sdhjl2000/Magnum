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
namespace Magnum.Channels
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Logging;

	/// <summary>
	/// A BroadcastChannel sends a message to zero or more listeners
	/// </summary>
	public class BroadcastChannel :
		UntypedChannel
	{
		private static readonly ILogger _log = Logger.GetLogger<BroadcastChannel>();

		private readonly UntypedChannel[] _listeners;

		public BroadcastChannel(IEnumerable<UntypedChannel> listeners)
		{
			Guard.AgainstNull(listeners, "listeners");

			_listeners = listeners.ToArray();
		}

		public BroadcastChannel(UntypedChannel[] listeners)
		{
			Guard.AgainstNull(listeners, "listeners");

			_listeners = listeners;
		}

		public IEnumerable<UntypedChannel> Listeners
		{
			get { return _listeners; }
		}

		public void Send<T>(T message)
		{
			foreach (UntypedChannel subscriber in _listeners)
			{
				try
				{
					subscriber.Send(message);
				}
				catch (Exception ex)
				{
					_log.Error(ex, "Send on a listener threw an exception");
				}
			}
		}
	}

	/// <summary>
	/// A BroadcastChannel sends a message to zero or more listeners
	/// </summary>
	/// <typeparam name = "T">Channel type</typeparam>
	public class BroadcastChannel<T> :
		Channel<T>
	{
		private static readonly ILogger _log = Logger.GetLogger<BroadcastChannel<T>>();

		private readonly Channel<T>[] _listeners;

		public BroadcastChannel(IEnumerable<Channel<T>> listeners)
		{
			Guard.AgainstNull(listeners, "listeners");

			_listeners = listeners.ToArray();
		}

		public BroadcastChannel(Channel<T>[] listeners)
		{
			Guard.AgainstNull(listeners, "listeners");

			_listeners = listeners;
		}

		public IEnumerable<Channel<T>> Listeners
		{
			get { return _listeners; }
		}

		public void Send(T message)
		{
			foreach (var channel in _listeners)
			{
				try
				{
					channel.Send(message);
				}
				catch (Exception ex)
				{
					_log.Error(ex, "Send on a listener threw an exception");
				}
			}
		}
	}
}