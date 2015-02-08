using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Reversi.Properties;
using ReversiProc;

namespace Reversi.Parts
{
    /// <summary>
    /// マスクラス
    /// </summary>
    public partial class Circle : Panel
    {
        #region [ プライベートインスタンス ]

        /// <summary>
        /// 画像リスト
        /// </summary>
        private Image[] m_imageList = new Image[6];

        /// <summary>
        /// 画像リストインデックス
        /// </summary>
        private int m_imgIdx = 0;

        #endregion
        
        #region [ アクセサ ]

        /// <summary>
        /// 表示色
        /// </summary>
        public Proc.ViewColor _ViewColor { set; get; }

        #endregion

        #region [ コンストラクタ ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <remarks>
        /// コンストラクタ処理
        /// </remarks>
        public Circle()
        {
            InitializeComponent();

            Reset();

            // イメージリストの設定
            this.m_imageList[0] = Resources.None;
            this.m_imageList[1] = Resources.None_o;
            this.m_imageList[2] = Resources.White;
            this.m_imageList[3] = Resources.White_o;
            this.m_imageList[4] = Resources.Black;
            this.m_imageList[5] = Resources.Black_o;
        }

        #endregion

        #region [ パブリックメソッド ]

        /// <summary>
        /// リセット表記
        /// </summary>
        public void Reset()
        {
            this._ViewColor = Proc.ViewColor.None;
            paintCircle();
        }

        /// <summary>
        /// 初回表示
        /// </summary>
        /// <param name="viewColor">色</param>
        public void InitView(Proc.ViewColor viewColor)
        {
            this._ViewColor = viewColor;
            paintCircle();
        }

        /// <summary>
        /// 反転
        /// </summary>
        public void Reverse()
        {
            if (this._ViewColor == Proc.ViewColor.Black)
            {
                this._ViewColor = Proc.ViewColor.White;
            }
            else if (this._ViewColor == Proc.ViewColor.White)
            {
                this._ViewColor = Proc.ViewColor.Black;
            }
            paintCircle();
        }

        #endregion

        #region [ コントロールイベント ]

        /// <summary>
        /// マウス領域内
        /// </summary>
        /// <remarks>
        /// マウスが領域内になったときの処理。
        /// 外枠線を白にする
        /// </remarks>
        /// <param name="sender">オブジェクト</param>
        /// <param name="e">イベント</param>
        private void Circle_MouseEnter(object sender, EventArgs e)
        {
            this.BackgroundImage = this.m_imageList[++this.m_imgIdx];
        }


        /// <summary>
        /// マウス領域外
        /// </summary>
        /// <remarks>
        /// マウスが領域外になったときの処理。
        /// 外枠線を黒にする
        /// </remarks>
        /// <param name="sender">オブジェクト</param>
        /// <param name="e">イベント</param>
        private void Circle_MouseLeave(object sender, EventArgs e)
        {
            this.BackgroundImage = this.m_imageList[--this.m_imgIdx];
        }

        #endregion

        #region [ プライベートメソッド ]

        /// <summary>
        /// 円描画
        /// </summary>
        private void paintCircle()
        {
            if (this._ViewColor == Proc.ViewColor.Black)
            {
                this.m_imgIdx = 4;
            }
            else if (this._ViewColor == Proc.ViewColor.None)
            {
                this.m_imgIdx = 0;
            }
            else if (this._ViewColor == Proc.ViewColor.White)
            {
                this.m_imgIdx = 2;
            }

            this.BackgroundImage = this.m_imageList[this.m_imgIdx];
        }

        #endregion
    }
}
