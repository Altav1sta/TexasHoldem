using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Engine.Objects
{
    internal class PlayersChain : IReadOnlyCollection<Player>
    {
        private readonly ReadOnlyCollection<Player> players;

        internal PlayersChain(IEnumerable<Player> players, int? seed)
        {
            var random = seed.HasValue ? new Random(seed.Value) : new Random(); 
            this.players = players.OrderBy(x => random.Next()).ToList().AsReadOnly();
            Dealer = random.Next(this.players.Count);
        }

        public int Dealer { get; private set; }

        public int Count => players.Count;

        public IEnumerator<Player> GetEnumerator()
        {
            return new PlayersEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool MoveDealer()
        {
            using (var enumerator = GetEnumerator())
            {
                if (!enumerator.MoveNext()) return false;

                Dealer = players.IndexOf(enumerator.Current);
            }

            return true;
        }

        private class PlayersEnumerator : IEnumerator<Player>
        {
            private readonly PlayersChain collection;
            private bool enumerated;
            private int position;

            internal PlayersEnumerator(PlayersChain collection)
            {
                this.collection = collection;
                Reset();
            }

            public bool MoveNext()
            {
                if (enumerated) return false;

                position++;

                if (position == collection.Count)
                {
                    position = 0;
                }

                if (position == collection.Dealer)
                {
                    enumerated = true;
                }

                return true;
            }

            public void Reset()
            {
                enumerated = false;
                position = collection.Dealer;
            }

            public Player Current => collection.players[position];

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }
        }
    }
}