using System;
using System.Collections.Generic;
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
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {

        #region [ プライベートインスタンス ]

        /// <summary>
        /// 各マス
        /// </summary>
        private Circle[,] m_circle = new Circle[8,8];

        /// <summary>
        /// 処理プロセス
        /// </summary>
        private Proc m_proc = null;

        /// <summary>
        /// 動作状態
        /// </summary>
        private Proc.PlayState m_playState = Proc.PlayState.None;

        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            this.m_proc = new Proc(new ReverseProc(reverseProc));

            // 初期設定
            this.m_circle = new Circle[,] { { this.circle1_1, this.circle2_1, this.circle3_1, this.circle4_1,
                                              this.circle5_1, this.circle6_1, this.circle7_1, this.circle8_1},
                                            { this.circle1_2, this.circle2_2, this.circle3_2, this.circle4_2,
                                              this.circle5_2, this.circle6_2, this.circle7_2, this.circle8_2},
                                            { this.circle1_3, this.circle2_3, this.circle3_3, this.circle4_3,
                                              this.circle5_3, this.circle6_3, this.circle7_3, this.circle8_3},
                                            { this.circle1_4, this.circle2_4, this.circle3_4, this.circle4_4,
                                              this.circle5_4, this.circle6_4, this.circle7_4, this.circle8_4},
                                            { this.circle1_5, this.circle2_5, this.circle3_5, this.circle4_5,
                                              this.circle5_5, this.circle6_5, this.circle7_5, this.circle8_5},
                                            { this.circle1_6, this.circle2_6, this.circle3_6, this.circle4_6,
                                              this.circle5_6, this.circle6_6, this.circle7_6, this.circle8_6},
                                            { this.circle1_7, this.circle2_7, this.circle3_7, this.circle4_7,
                                              this.circle5_7, this.circle6_7, this.circle7_7, this.circle8_7},
                                            { this.circle1_8, this.circle2_8, this.circle3_8, this.circle4_8,
                                              this.circle5_8, this.circle6_8, this.circle7_8, this.circle8_8}
            };

            // 初期設定
            for (int i = 0; i < this.m_circle.GetLength(0); i++)
            {
                for (int j = 0; j < this.m_circle.GetLength(1); j++)
                {
                    this.m_circle[i, j].Reset();
                    // タグに位置情報を設定する
                    this.m_circle[i, j].Tag = i + "," + j;
                }
            }

            // ラベル初期化
            this.stateLabel.Content = "";
            this.skpbutton.IsEnabled = false;
        }

        #region [ コントロールイベント ]

        /// <summary>
        /// startボタンクリック時イベント
        /// </summary>
        /// <param name="sender">オブジェクト</param>
        /// <param name="e">イベント</param>
        private void stbutton_Click(object sender, RoutedEventArgs e)
        {
            if (this.m_playState == Proc.PlayState.None)
            {
                // メッセージを表示したのち、
                // 開始状態にする
                for (int iX = 0; iX < this.m_circle.GetLength(0); iX++)
                {
                    for (int iY = 0; iY < this.m_circle.GetLength(1); iY++)
                    {
                        this.m_circle[iX, iY].Reset();
                    }
                }
                // 座標（4, 4）（5, 5）に白、（4, 5）（5, 4）に黒の石を設定する
                this.m_circle[3, 3].InitView(Proc.ViewColor.White);
                this.m_circle[4, 4].InitView(Proc.ViewColor.White);
                this.m_circle[3, 4].InitView(Proc.ViewColor.Black);
                this.m_circle[4, 3].InitView(Proc.ViewColor.Black);

                // 初回ターンは白から
                this.m_proc._Turn = Proc.Turn.Black;
                this.stateLabel.Content = this.m_proc.ReverseTurn();
                // ボタン文字変更
                this.stbutton.Content = "E  n  d";
                this.m_playState = Proc.PlayState.Play;
                this.skpbutton.IsEnabled = true;
            }
            else if (this.m_playState == Proc.PlayState.Play)
            {
                // 終了処理
                finProc();
            }
            else if (this.m_playState == Proc.PlayState.End)
            {
                // 全クリア
                for (int iX = 0; iX < this.m_circle.GetLength(0); iX++)
                {
                    for (int iY = 0; iY < this.m_circle.GetLength(1); iY++)
                    {
                        this.m_circle[iX, iY].Reset();
                    }
                }
                this.stbutton.Content = "S t a r t";
                this.m_playState = Proc.PlayState.None;
                this.skpbutton.IsEnabled = true;
                this.stateLabel.Content = "";
            }
        }

        /// <summary>
        /// Skipボタン押下時イベント
        /// </summary>
        /// <param name="sender">オブジェクト</param>
        /// <param name="e">イベント</param>
        private void skpbutton_Click(object sender, RoutedEventArgs e)
        {
            // 今のターンの人を飛ばす
            this.stateLabel.Content = this.m_proc.ReverseTurn();
        }

        /// <summary>
        /// クリック時イベント
        /// </summary>
        /// <param name="sender">オブジェクト</param>
        /// <param name="e">イベント</param>
        private void circle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // 左クリックのみ、かつプレー中のみ有効
            if (e.ChangedButton == MouseButton.Left && this.m_playState == Proc.PlayState.Play)
            {
                Circle circle = (Circle)sender;
                string[] strTagData = circle.Tag.ToString().Split(new char[] { ',' });

                // タグ情報によって石状態を取得
                int iX = int.Parse(strTagData[0]);
                int iY = int.Parse(strTagData[1]);

                if (!this.m_proc.ClickProc(getViewColor(), iX, iY))
                {
                    MessageBox.Show(this.m_proc._ErrMsg, "ERROR");
                }
                else
                {
                    // 石を置く
                    Proc.ViewColor color = Proc.ViewColor.None;
                    if (this.m_proc._Turn == Proc.Turn.Black)
                    {
                        color = Proc.ViewColor.Black;
                    }
                    else
                    {
                        color = Proc.ViewColor.White;
                    }
                    this.m_circle[iX, iY].InitView(color);
                    // ターン変更
                    this.stateLabel.Content = this.m_proc.ReverseTurn();

                    // 終了確認を行う
                    if (this.m_proc.FinCheck(getViewColor()))
                    {
                        // 終了処理
                        finProc();
                    }
                }
            }
        }

        #endregion

        #region [ プライベートメソッド ]

        /// <summary>
        /// 表示色一覧取得処理
        /// </summary>
        /// <returns>表示色一覧</returns>
        private Proc.ViewColor[,] getViewColor()
        {
            Proc.ViewColor[,] viewColor = new Proc.ViewColor[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    viewColor[i, j] = this.m_circle[i, j]._ViewColor;
                }
            }
            return viewColor;
        }

        /// <summary>
        /// 反転処理
        /// </summary>
        /// <param name="iX">開始X</param>
        /// <param name="iY">開始Y</param>
        /// <param name="iXAdd">X方向加算値</param>
        /// <param name="iYAdd">Y方向加算値</param>
        /// <param name="iCount">加算回数</param>
        private void reverseProc(int iX, int iY, int iXAdd, int iYAdd, int iCount)
        {
            for (int x = iX + iXAdd, y = iY + iYAdd, iStep = 1; iStep < iCount; x += iXAdd, y += iYAdd, iStep++)
            {
                this.m_circle[x, y].Reverse();
            }
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        private void finProc()
        {
            int iWhite = 0,iBlack = 0;
            // 集計後
            this.m_proc.TotalProc(getViewColor(), out iWhite, out iBlack);
            this.stbutton.Content = "R e s e t";
            this.m_playState = Proc.PlayState.End;
            this.skpbutton.IsEnabled = false;
            this.stateLabel.Content = "白：" + iWhite.ToString() + "　黒：" + iBlack.ToString();
        }

        #endregion

    }
}
