<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:template match="/ArrayOfOrderDetails">
    <html>
      <head>
        <title>订单列表</title>
      </head>
      <body>
        <ul>
          <xsl:for-each select="OrderDetails">
            <li>
              <font face="黑体">购买时间: </font>
              <xsl:value-of select="OrderTime" />
              <br />
              <font face="黑体">订单号: </font>
              <xsl:value-of select="OrderNumber" />
              <br />

              <xsl:for-each select="Customer">
                <font face="黑体">顾客姓名: </font>
                <xsl:value-of select="CustomerName" />
                <br />
                <font face="黑体">联系方式: </font>
                <xsl:value-of select="PhoneNumber" />
                <br />
              </xsl:for-each>

              <font face="黑体">货物清单: </font>
              <xsl:for-each select="Goods">
                <xsl:for-each select="Goods">
                  <br />
                  <font face="黑体" color="grey">货物序号: </font>
                  <xsl:value-of select="GoodsNumber" />
                  <br />
                  <font face="黑体" color="grey">商品名: </font>
                  <xsl:value-of select="TradeName" />
                  <br />
                  <font face="黑体" color="grey">类型: </font>
                  <xsl:value-of select="Type" />
                  <br />
                  <font face="黑体" color="grey">单价: </font>
                  <xsl:value-of select="UnitPrice" />
                  <br />
                  <font face="黑体" color="grey">数量: </font>
                  <xsl:value-of select="Count" />
                  <br />
                  <font face="黑体" color="grey">总价: </font>
                  <xsl:value-of select="TotalPrice" />
                  <br />
                </xsl:for-each>
              </xsl:for-each>
            </li>
            <br />
            <br />
          </xsl:for-each>
        </ul>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>