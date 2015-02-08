using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ReversiProc;

namespace ReversiWpf
{
    /// <summary>
    /// Circle.xaml の相互作用ロジック
    /// </summary>
    public partial class Circle : UserControl
    {


        #region [ プライベートインスタンス ]

        /// <summary>
        /// 表示色
        /// </summary>
        private Proc.ViewColor m_viewColor = Proc.ViewColor.None;

        #endregion

        #region [ アクセサ ]

        /// <summary>
        /// 表示色
        /// </summary>
        public Proc.ViewColor _ViewColor
        {
            get
            {
                return this.m_viewColor;
            }
        }

        #endregion

        #region [ コンストラクタ ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Circle()
        {
            InitializeComponent();
            Reset();
            this.BorderThickness = new Thickness(1);
        }

        #endregion

        #region [ パブリックメソッド ]

        /// <summary>
        /// リセット表記
        /// </summary>
        public void Reset()
        {
            this.m_viewColor = Proc.ViewColor.None;
            paintCircle();
        }

        /// <summary>
        /// 初回表示
        /// </summary>
        /// <param name="viewColor">色</param>
        public void InitView(Proc.ViewColor viewColor)
        {
            this.m_viewColor = viewColor;
            paintCircle();
        }

        /// <summary>
        /// 反転
        /// </summary>
        public void Reverse()
        {
            if (this.m_viewColor == Proc.ViewColor.Black)
            {
                this.m_viewColor = Proc.ViewColor.White;
            }
            else if (this.m_viewColor == Proc.ViewColor.White)
            {
                this.m_viewColor = Proc.ViewColor.Black;
            }
            paintCircle();
        }

        #endregion

        #region [ プライベートメソッド ]

        /// <summary>
        /// 円描画
        /// </summary>
        private void paintCircle()
        {
            if (this.m_viewColor == Proc.ViewColor.Black)
            {
                this.ellipseB.Visibility = Visibility.Visible;
                this.ellipseW.Visibility = Visibility.Hidden;
            }
            else if (this.m_viewColor == Proc.ViewColor.None)
            {
                this.ellipseB.Visibility = Visibility.Hidden;
                this.ellipseW.Visibility = Visibility.Hidden;
            }
            else if (this.m_viewColor == Proc.ViewColor.White)
            {
                this.ellipseB.Visibility = Visibility.Hidden;
                this.ellipseW.Visibility = Visibility.Visible;
            }
        }

        #endregion

        #region [ コントロールイベント ]

        /// <summary>
        /// マウスカーソルが領域内に入ってきたとき
        /// </summary>
        /// <param name="sender">オブジェクト</param>
        /// <param name="e">イベント</param>
        private void Circle_MouseEnter(object sender, MouseEventArgs e)
        {
            // 枠線描画
            this.BorderBrush = Brushes.White;
        }

        /// <summary>
        /// マウスカーソルが外れたとき
        /// </summary>
        /// <param name="sender">オブジェクト</param>
        /// <param name="e">イベント</param>
        private void Circle_MouseLeave(object sender, MouseEventArgs e)
        {
            this.BorderBrush = Brushes.Black;
        }

        #endregion
    }
}
