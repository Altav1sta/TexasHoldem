using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Objects
{
    public class PlayersChain : IReadOnlyList<Player>
    {
        private readonly Player[] players;

        internal PlayersChain(IEnumerable<Player> players, int? seed)
        {
            var random = seed.HasValue ? new Random(seed.Value) : new Random(); 
            this.players = players.OrderBy(x => random.Next()).ToArray();
            Dealer = random.Next(this.players.Length);
        }

        public Player this[int index]
        {
            get
            {
                if (index < 0) throw new ArgumentOutOfRangeException();

                index = index % players.Length;
                
                foreach (var player in players)
                {
                    index--;

                    if (index < 0) return player;
                }
                
                throw new Exception("Player not found");
            }
        }

        public Player this[string id]
        {
            get { return players.FirstOrDefault(x => x.Id == id); }
        }
        
        public int Dealer { get; private set; }

        public int Count => players.Length;

        public IEnumerator<Player> GetEnumerator()
        {
            return new PlayersEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int IndexOf(Player player)
        {
            return Array.IndexOf(players, player);
        }
        
        public bool MoveDealer()
        {
            using (var enumerator = GetEnumerator())
            {
                if (!enumerator.MoveNext()) return false;

                Dealer = Array.IndexOf(players, enumerator.Current);
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
                position++;

                if (position == collection.Count) position = 0;
                
                if (enumerated) return false;
                
                if (position == collection.Dealer) enumerated = true;

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