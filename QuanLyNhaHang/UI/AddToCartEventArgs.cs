using System;
using QuanLyNhaHang.Models;

namespace QuanLyNhaHang.UI
{
    public class AddToCartEventArgs : EventArgs
    {
        public ThucDonViewModel Mon { get; }
        public int SoLuong { get; }

        public AddToCartEventArgs(ThucDonViewModel mon, int soLuong)
        {
            Mon = mon;
            SoLuong = soLuong;
        }
    }
}
