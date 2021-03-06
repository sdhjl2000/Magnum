// Copyright 2007-2010 The Apache Software Foundation.
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
namespace Magnum.Actors.Internal
{
	using System;
	using System.Collections.Generic;
	using Channels;
	using Extensions;


	public class WithinSentRequestImpl<TRequest> :
		WithinSentRequest<TRequest>
	{
		readonly Inbox _inbox;
		readonly IList<PendingReceive> _receives;
		readonly TimeSpan _timeout;
		bool _handled;
		Action _timeoutCallback = DoNothing;

		public WithinSentRequestImpl(Inbox inbox, TimeSpan timeout)
		{
			_inbox = inbox;
			_timeout = timeout;

			_receives = new List<PendingReceive>();
		}

		public WithinSentRequest<TRequest> Receive<T>(SelectiveConsumer<T> consumer)
		{
			PendingReceive receive = _inbox.Receive<T>(candidate =>
				{
					Consumer<T> accepted = consumer(candidate);
					if (accepted == null)
						return null;

					return message =>
						{
							accepted(message);

							Complete();
						};
				}, _timeout, HandleTimeout);

			_receives.Add(receive);

			return this;
		}

		public WithinSentRequest<TRequest> Otherwise(Action timeoutCallback)
		{
			_timeoutCallback = timeoutCallback;

			return this;
		}

		void HandleTimeout()
		{
			if (_handled)
				return;

			_timeoutCallback();

			Complete();
		}

		void Complete()
		{
			RemovePendingReceives();

			_handled = true;
		}

		void RemovePendingReceives()
		{
			_receives.Each(x => x.Cancel());
			_receives.Clear();
		}

		static void DoNothing()
		{
		}
	}
}