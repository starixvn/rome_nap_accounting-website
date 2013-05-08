<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:param name="IsInternal" select="1"></xsl:param>
  <xsl:output method="html"/>
  <xsl:template match="acc_customerupdmember_commission">
    <table class="tblDesc">
      <tr>
        <td class="colW1">{lbl:playerdiscount}</td>
        <td class="colW2">
          <xsl:value-of select="@playerdiscount" />
        </td>

        <td class="colW1">{lbl:playerdiscountcs}</td>
        <td class="colW2">
          <xsl:value-of select="@playerdiscountcs" />
        </td>
        <td class="colW1">{lbl:playerdiscount1x2}</td>
        <td class="colW2">
          <xsl:value-of select="@playerdiscount1x2" />
        </td>
      </tr>
      <tr>
        <td class="colW1">{lbl:playerdiscountnumber}</td>
        <td class="colW2">
          <xsl:value-of select="@playerdiscountnumber" />
        </td>
        <td class="colW1">{lbl:language}</td>
        <td class="colW2">
          <xsl:value-of select="@language" />
        </td>
        <td class="colW1">{lbl:ip}</td>
        <td class="colW2">
          <xsl:value-of select="@ip" />
        </td>
        
      </tr>
      <tr>
        <td class="colW1">{lbl:errcode}</td>
        <td class="colW2">
          <xsl:value-of select="@errcode" />
        </td>
        <td class="colW1">{lbl:errmsg}</td>
        <td class="colW2">
          <xsl:value-of select="@errmsg" />
        </td>
        <xsl:choose>
          <xsl:when test="@subaccname">
              <td class="colW1">{lbl:subaccname}</td>
              <td class="colW2">
                <xsl:value-of select="@subaccname" />
              </td>
          </xsl:when>
          <xsl:otherwise>
            <td class="colTitle"></td>
            <td class="colValue">

            </td>
          </xsl:otherwise>
        </xsl:choose>
        
      </tr>
    </table>
  </xsl:template>
</xsl:stylesheet>

