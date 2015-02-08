using System;
using System.Collections.Generic;
using System.Text;

namespace ReversiProc
{
    /// <summary>
    /// 反転処理
    /// </summary>
    /// <param name="iX">開始X</param>
    /// <param name="iY">開始Y</param>
    /// <param name="iXAdd">X方向加算値</param>
    /// <param name="iYAdd">Y方向加算値</param>
    /// <param name="iCount">加算回数</param>
    public delegate void ReverseProc(int iX, int iY, int iXAdd, int iYAdd, int iCount);

    public class Proc
    {
        #region [ パブリック定数 ]

        /// <summary>
        /// 表示色
        /// </summary>
        public enum ViewColor
        {
            /// <summary>
            /// なし
            /// </summary>
            None,
            /// <summary>
            /// 黒
            /// </summary>
            Black,
            /// <summary>
            /// 白
            /// </summary>
            White
        }

        /// <summary>
        /// ターン情報
        /// </summary>
        public enum Turn
        {
            /// <summary>
            /// 白のターン
            /// </summary>
            White,
            /// <summary>
            /// 黒のターン
            /// </summary>
            Black
        }

        /// <summary>
        /// チェックステータス
        /// </summary>
        public enum CheckState
        {
            /// <summary>
            /// 石なし
            /// </summary>
            None,
            /// <summary>
            /// 同色石
            /// </summary>
            Same,
            /// <summary>
            /// 反色石
            /// </summary>
            Different
        }

        /// <summary>
        /// 動作状態
        /// </summary>
        public enum PlayState
        {
            /// <summary>
            /// 待ち
            /// </summary>
            None,
            /// <summary>
            /// プレイ中
            /// </summary>
            Play,
            /// <summary>
            /// 終了
            /// </summary>
            End
        }

        /// <summary>
        /// 盤、X方向サイズ
        /// </summary>
        public const int CIRCLE_X = 8;

        /// <summary>
        /// 盤、Y方向サイズ
        /// </summary>
        public const int CIRCLE_Y = 8;


        #endregion

        #region [ プライベートインスタンス ]


        /// <summary>
        /// 表示色配列
        /// </summary>
        private ViewColor[,] m_viewColor = null;

        /// <summary>
        /// 反転処理デリゲート
        /// </summary>
        private ReverseProc m_reverseProc = null;

        #endregion

        #region [ アクセサ ]

        /// <summary>
        /// ターン
        /// </summary>
        public Turn _Turn { set; get; }

        /// <summary>
        /// エラーメッセージ
        /// </summary>
        public string _ErrMsg { set; get; }

        #endregion

        #region [ コンストラクタ ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="reverseProc">反転処理</param>
        public Proc(ReverseProc reverseProc)
        {
            this.m_reverseProc = reverseProc;
        }

        #endregion

        #region [ パブリックメソッド ]

        /// <summary>
        /// クリック時処理
        /// </summary>
        /// <param name="viewColor"></param>
        /// <param name="iX"></param>
        /// <param name="iY"></param>
        /// <returns></returns>
        public bool ClickProc(ViewColor[,] viewColor, int iX, int iY)
        {
            this.m_viewColor = viewColor;
            // 反転
            bool blRet = false;
            if (!reverse(iX, iY))
                this._ErrMsg = "ここに石を置くことはできません";
            else
                blRet = true;
            return blRet;
        }

        /// <summary>
        /// ターンチェンジ処理
        /// </summary>
        public string ReverseTurn()
        {
            if (this._Turn == Turn.White)
            {
                this._Turn = Turn.Black;
                return "黒のターンです";
            }
            else
            {
                this._Turn = Turn.White;
                return "白のターンです";
            }
        }

        /// <summary>
        /// 終了チェック
        /// </summary>
        /// <returns>BOOL型</returns>
        public bool FinCheck(ViewColor[,] viewColor)
        {
            // 全てどちらかの色しかなくなっている場合
            // または
            // 全マスが埋まった場合
            bool blWhite = true;
            bool blBlack = true;
            bool blFixCheck = true;
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (viewColor[i, j] == Proc.ViewColor.Black)
                        blBlack = false;
                    else if (viewColor[i, j] == Proc.ViewColor.White)
                        blWhite = false;
                    else
                        blFixCheck = false;
            if (blFixCheck)
                return true;
            else if (blBlack || blWhite)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 集計処理
        /// </summary>
        /// <param name="viewColor">表示色一覧</param>
        /// <param name="iWhite">白数</param>
        /// <param name="iBlack">黒数</param>
        public void TotalProc(ViewColor[,] viewColor, out int iWhite, out int iBlack)
        {
            iWhite = iBlack = 0;
            // 集計処理
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (viewColor[i, j] == Proc.ViewColor.Black)
                        iBlack++;
                    else if (viewColor[i, j] == Proc.ViewColor.White)
                        iWhite++;
        }


        #endregion

        #region [ プライベートメソッド ]

        /// <summary>
        /// クリックした箇所に石が置かれていないか確認する
        /// </summary>
        /// <param name="iX">クリック箇所（X）</param>
        /// <param name="iY">クリック箇所（Y）</param>
        /// <returns>BOOL型</returns>
        private bool reverse(int iX, int iY)
        {
            bool[] blResult = new bool[8];
            // そもそも石が表示されているところは置くことができない
            bool blRet = this.m_viewColor[iX, iY] != ViewColor.None ? false : true;
            if (blRet)
            {
                // 方向と配列インデックスの関係性
                // 0=左上
                // 1=上
                // 2=右上
                // 3=左
                // 4=右
                // 5=左下
                // 6=下
                // 7=右下
                int iIdx = 0;
                for (int x = -1; x <= 1; x++)
                    for (int y = -1; y <= 1; y++)
                        if (!checkReverse(x, y))
                            blResult[iIdx++] = false;
                        else
                            blResult[iIdx++] = checkEndPoint(iIdx, iX, iY);
                return Array.IndexOf(blResult, true) != -1 ? false : true;
            }
            else
                return false;
        }

        /// <summary>
        /// ひっくり返せるかどうがをチェックする
        /// （反対色があるか確認）
        /// </summary>
        /// <param name="iX">チェックするX</param>
        /// <param name="iY">チェックするY</param>
        /// <returns>BOOL型</returns>
        private bool checkReverse(int iX, int iY)
        {
            // 範囲外なら除外
            if (iX < 0 || iX > 7 || iY < 0 || iY > 7)
                return false;
            // 石がなければ除外
            if (this.m_viewColor[iX, iY] == ViewColor.None)
                return false;
            // ひっくり返せるならtrueを返す
            if ((this.m_viewColor[iX, iY] == ViewColor.Black && this._Turn == Turn.White) ||
                    (this.m_viewColor[iX, iY] == ViewColor.White && this._Turn == Turn.Black))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 方向に伴う先に同じ色の石があるか確認する
        /// </summary>
        /// <param name="iWay">方向</param>
        /// <param name="iX">クリック箇所（X）</param>
        /// <param name="iY">クリック箇所（Y）</param>
        /// <returns>BOOL型</returns>
        private bool checkEndPoint(int iWay, int iX, int iY)
        {
            bool blSetFlg = false;

            switch (iWay)
            {
                case 0:
                    for (int i = iX - 1, j = iY - 1, iStep = 1; i >= 0 && j >= 0; i--, j--, iStep++)
                    {
                        CheckState state = checkReverse2(i, j);
                        // 石がなくなるのなら終わり
                        if (state == CheckState.None)
                            break;
                        // 同じ色がきたら
                        if (state == CheckState.Same)
                        {
                            // ひっくり返す
                            this.m_reverseProc(iX, iY, -1, -1, iStep);
                            blSetFlg = true;
                            break;
                        }
                    }
                    break;
                case 1:
                    for (int i = iY - 1, iStep = 1; i >= 0; i--, iStep++)
                    {
                        CheckState state = checkReverse2(iX, i);
                        // 石がなくなるのなら終わり
                        if (state == CheckState.None)
                            break;
                        // 同じ色がきたら
                        if (state == CheckState.Same)
                        {
                            // ひっくり返す
                            this.m_reverseProc(iX, iY, 0, -1, iStep);
                            blSetFlg = true;
                            break;
                        }
                    }
                    break;
                case 2:
                    for (int i = iX + 1, j = iY - 1, iStep = 1; i < 8 && j >= 0; i++, j--, iStep++)
                    {
                        CheckState state = checkReverse2(i, j);
                        // 石がなくなるのなら終わり
                        if (state == CheckState.None)
                            break;
                        // 同じ色がきたら
                        if (state == CheckState.Same)
                        {
                            // ひっくり返す
                            this.m_reverseProc(iX, iY, 1, -1, iStep);
                            blSetFlg = true;
                            break;
                        }
                    }
                    break;
                case 3:
                    for (int i = iX - 1, iStep = 1; i >= 0; i--, iStep++)
                    {
                        CheckState state = checkReverse2(i, iY);
                        // 石がなくなるのなら終わり
                        if (state == CheckState.None)
                            break;
                        // 同じ色がきたら
                        if (state == CheckState.Same)
                        {
                            // ひっくり返す
                            this.m_reverseProc(iX, iY, -1, 0, iStep);
                            blSetFlg = true;
                            break;
                        }
                    }
                    break;
                case 4:
                    for (int i = iX + 1, iStep = 1; i < 8; i++, iStep++)
                    {
                        CheckState state = checkReverse2(i, iY);
                        // 石がなくなるのなら終わり
                        if (state == CheckState.None)
                            break;
                        // 同じ色がきたら
                        if (state == CheckState.Same)
                        {
                            // ひっくり返す
                            this.m_reverseProc(iX, iY, 1, 0, iStep);
                            blSetFlg = true;
                            break;
                        }
                    }
                    break;
                case 5:
                    for (int i = iX - 1, j = iY + 1, iStep = 1; i >= 0 && j < 8; i--, j++, iStep++)
                    {
                        CheckState state = checkReverse2(i, j);
                        // 石がなくなるのなら終わり
                        if (state == CheckState.None)
                            break;
                        // 同じ色がきたら
                        if (state == CheckState.Same)
                        {
                            // ひっくり返す
                            this.m_reverseProc(iX, iY, -1, 1, iStep);
                            blSetFlg = true;
                            break;
                        }
                    }
                    break;
                case 6:
                    for (int i = iY + 1, iStep = 1; i < 8; i++, iStep++)
                    {
                        CheckState state = checkReverse2(iX, i);
                        // 石がなくなるのなら終わり
                        if (state == CheckState.None)
                            break;
                        // 同じ色がきたら
                        if (state == CheckState.Same)
                        {
                            // ひっくり返す
                            this.m_reverseProc(iX, iY, 0, 1, iStep);
                            blSetFlg = true;
                            break;
                        }
                    }
                    break;
                case 7:
                    for (int i = iX + 1, j = iY + 1, iStep = 1; i < 8 && j < 8; i++, j++, iStep++)
                    {
                        CheckState state = checkReverse2(i, j);
                        // 石がなくなるのなら終わり
                        if (state == CheckState.None)
                            break;
                        // 同じ色がきたら
                        if (state == CheckState.Same)
                        {
                            // ひっくり返す
                            this.m_reverseProc(iX, iY, 1, 1, iStep);
                            blSetFlg = true;
                            break;
                        }
                    }
                    break;
            }

            return blSetFlg;
        }

        private bool checkReversePoint(int iX, int iY, int iXAdd, int iYAdd, int iXEnd, int iYEnd, int iXPoint, int iYPoint)
        {
            for (int i=iX,j=iY,iStep = 1; i<iXEnd && j<iYEnd;i+=iXAdd,j+=iYEnd,iStep++)
            {
                CheckState state = checkReverse2(i, j);
                // 石がなくなるのなら終わり
                if (state == CheckState.None)
                    break;
                // 同じ色がきたら
                if (state == CheckState.Same)
                {
                    // ひっくり返す
                    this.m_reverseProc(iX, iY, 1, 1, iStep);
                    return true;
                }
            }
            return false;
        }



        /// <summary>
        /// ひっくり返せるかどうがをチェックする
        /// （同じ色があるか確認）
        /// </summary>
        /// <param name="iX">チェックするX</param>
        /// <param name="iY">チェックするY</param>
        /// <returns>BOOL型</returns>
        private CheckState checkReverse2(int iX, int iY)
        {
            // 石がなければ除外
            if (this.m_viewColor[iX, iY] == Proc.ViewColor.None)
            {
                return CheckState.None;
            }
            if ((this.m_viewColor[iX, iY] == ViewColor.Black && this._Turn == Turn.Black) ||
                    (this.m_viewColor[iX, iY] == ViewColor.White && this._Turn == Turn.White))
            {
                return CheckState.Same;
            }
            else
            {
                return CheckState.Different;
            }
        }


        /// <summary>
        /// 現在のターンと反対のターンを取得
        /// </summary>
        /// <returns>反対ターン</returns>
        private Turn getReverseTurn()
        {
            // ターン変更
            if (this._Turn == Turn.Black)
            {
                return Turn.White;
            }
            else
            {
                return Turn.Black;
            }
        }

        #endregion
    }
}
