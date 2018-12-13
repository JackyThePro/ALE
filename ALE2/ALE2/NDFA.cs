using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALE2
{
    public class NDFA
    {
        
        private List<Node> tree;
        private int StateCount;
        private IList<State> States;

        public NDFA(List<Node> tree, int stateCount, IList<State> states)
        {
            this.tree = tree;
            this.States = states;
            this.StateCount = stateCount;
        }

        public void Selectf(Node node)
        {
            switch (node.Param)
            {
                case '|':
                    fOr(tree[node.ChildOneId-1], tree[node.ChildTwoId - 1]);
                    break;
                case '.':
                    fAnd(tree[node.ChildOneId - 1], tree[node.ChildTwoId - 1]);
                    break;
                case '*':
                    fLoop(tree[node.ChildOneId - 1]);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void fLoop(Node node)
        {
            int fixedCount = StateCount;
            if (!node.Letter)
            {
                Selectf(node);
                CreateTransition(StateCount, fixedCount);
                CreateTransition(StateCount);
                CreateTransition(fixedCount, StateCount);
            }
            else
            {
                CreateTransition(StateCount, fixedCount, node.Param);
            }
        }

        private void fAnd(Node ChildOneId, Node ChildTwoId)
        {
            if (!ChildOneId.Letter)
            {
                Selectf(ChildOneId);
            }
            else
            {
                CreateTransition(StateCount, ChildOneId.Param);
            }

            if (!ChildTwoId.Letter)
            {
                Selectf(ChildTwoId);
            }
            else
            {
                CreateTransition(StateCount, ChildTwoId.Param);
            }
        }

        private void fOr(Node ChildOneId, Node ChildTwoId)
        {
            int fixedCount = StateCount;

            if (ChildOneId.Letter)
            {
                CreateTransition(StateCount, ChildOneId.Param);
                if (ChildTwoId.Letter)
                {
                    CreateTransition(fixedCount, fixedCount + 1, ChildTwoId.Param);
                    return;
                }
            }
            else
            {
                CreateTransition(StateCount);
                Selectf(ChildOneId);
            }

            int lastChildOneIdCount = StateCount;

            if (ChildTwoId.Letter)
            {
                CreateTransition(fixedCount, ChildTwoId.Param);
            }
            else
            {
                CreateTransition(fixedCount);
                Selectf(ChildTwoId);
            }

            // затварящи стрелки
            CreateTransition(StateCount);
            CreateTransition(lastChildOneIdCount, StateCount);
        }

        private void CreateTransition(int startCount, char letter = '_')
        {
            var endState = new State((StateCount+1).ToString(),false,false);
            States.Add(endState);
            CreateTransition(startCount, endState, letter);
            StateCount++;
        }

        private void CreateTransition(int startIndex, int endIndex, char letter = '_')
        {
            CreateTransition(startIndex, States[endIndex-1], letter);
        }

        private void CreateTransition(int startIndex, State endState, char letter = '_')
        {
            var transition = new Transmission(States[startIndex-1], endState, letter.ToString());

            States[startIndex-1].outgoing.Add(transition);
        }
    }
}
