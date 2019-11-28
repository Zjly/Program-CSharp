using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem {
    /// <summary>
    /// 商品类
    /// </summary>
    public class Goods {
		/// <summary>
		/// 商品编号
		/// </summary>
        [Key]
		public string GoodsNumber { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string TradeName { get; set; }

        /// <summary>
        /// 商品类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 商品单价
        /// </summary>
        public double UnitPrice { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        public int Count { get; set; }

		/// <summary>
        /// 商品总价
        /// </summary>
		public double TotalPrice { get; set; }

		/// <summary>
        /// 初始化商品信息
        /// </summary>
		public Goods() {
			// 创建商品类时自动初始化商品的编号
			GoodsNumber = Guid.NewGuid().ToString();
		}

        /// <summary>
        /// 商品类信息显示
        /// </summary>
        /// <returns>商品信息概要</returns>
        public override string ToString() {
			return $"商品名:{TradeName} 类型:{Type} 单价:{UnitPrice}";
		}
	}
}