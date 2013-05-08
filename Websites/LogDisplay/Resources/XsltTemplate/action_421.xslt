<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:param name="IsInternal" select="1"></xsl:param>
    <xsl:output method="html"/>
  <xsl:template match="acc_customerupdmemberhr_positiontaking">
    <table class="tblDesc">
      <xsl:for-each select="info">
        <tr class="headerDetailNormal" >
          <td colspan="6">{lbl:custInfo}</td>
        </tr>
        <tr >

          <td class="colW1">{lbl:IP}</td>
          <td class="colW2">
            <xsl:value-of select="@ip" />
          </td>

          <td class="colW1">{lbl:Language}</td>
          <td class="colW2">
            <xsl:value-of select="@language" />
          </td>
          <td class="colW1">{lbl:SubAccName}</td>
          <td class="colW2">
            <xsl:value-of select="@subaccname" />
          </td>
        </tr>
        <tr >
          <td class="colW1">{lbl:ErrCode}</td>
          <td class="colW2">
            <xsl:value-of select="@errcode" />
          </td>

          <td class="colW1">{lbl:ErrMsg}</td>
          <td class="colW2">
            <xsl:value-of select="@errmsg" />
          </td>
          <td class="colW1"></td>
          <td class="colW2">

          </td>
        </tr>


      </xsl:for-each>
      <xsl:for-each select="ag">
        <tr class="headerDetailNormal" >
          <td colspan="6">{lbl:ag}</td>
        </tr>
        <tr>
          <td class="colTitle">{lbl:v151apt_31}</td>
          <td class="colW2">
            <xsl:value-of select="@v151apt_31" />
          </td>
          <td class="colTitle">{lbl:v151apt_32}</td>
          <td class="colW2">
            <xsl:value-of select="@v151apt_32" />
          </td>

          <td class="colTitle">{lbl:v151apt_41}</td>
          <td class="colW2">
            <xsl:value-of select="@v151apt_41" />
          </td>
        </tr>
        <tr>
          <td class="colTitle">{lbl:v151apt_42}</td>
          <td class="colW2">
            <xsl:value-of select="@v151apt_42" />
          </td>
          <td class="colTitle">{lbl:v151apt_51}</td>
          <td class="colW2">
            <xsl:value-of select="@v151apt_51" />
          </td>
          <td class="colTitle">{lbl:v151apt_52}</td>
          <td class="colW2">
            <xsl:value-of select="@v151apt_52" />
          </td>
        </tr>
        <tr>
          <td class="colTitle">{lbl:v152apt_41}</td>
          <td class="colW2">
            <xsl:value-of select="@v152apt_41" />
          </td>
          <td class="colTitle">{lbl:v152apt_42}</td>
          <td class="colW2">
            <xsl:value-of select="@v152apt_42" />
          </td>
          <td class="colTitle">{lbl:v152apt_51}</td>
          <td class="colW2">
            <xsl:value-of select="@v152apt_51" />
          </td>
        </tr>
        <tr>
          <td class="colTitle">{lbl:v152apt_52}</td>
          <td class="colW2">
            <xsl:value-of select="@v152apt_52" />
          </td>
          <td class="colTitle">{lbl:v153apt_51}</td>
          <td class="colW2">
            <xsl:value-of select="@v153apt_51" />
          </td>
          <td class="colTitle">{lbl:v153apt_52}</td>
          <td class="colW2">
            <xsl:value-of select="@v153apt_52" />
          </td>
        </tr>

      </xsl:for-each>
    </table>
  </xsl:template>
</xsl:stylesheet>
