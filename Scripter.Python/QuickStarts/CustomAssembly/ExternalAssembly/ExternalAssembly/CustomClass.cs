using System.Windows.Forms;

namespace ExternalAssembly
{
    /// <summary>
    /// just test class
    /// </summary>
    public class CustomClass
    {
        /// <summary>
        /// test method
        /// </summary>
        /// <param name="firstParam">first param</param>
        /// <param name="secondParam">second param</param>
        public void TestMethod(int firstParam, bool secondParam, object thirdParam)
        {
            MessageBox.Show(thirdParam.GetType().FullName);
            MessageBox.Show(string.Format("first param is {0}, second param is {1}", firstParam, secondParam));
        }

        /// <summary>
        /// test prop
        /// </summary>
        public int TestProperty { get; set; }
    }
}
