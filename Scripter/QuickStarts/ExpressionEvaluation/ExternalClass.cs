using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionEvaluation
{
    public class ExternalClass
    {
        private MainForm owner;

        internal ExternalClass(MainForm owner)
        {
            this.owner = owner;
        }

        public string Text
        {
            get
            {
                return owner.TextBoxExpression.Text;
            }

            set
            {
                owner.TextBoxExpression.Text = value;
            }
        }
    }
}
