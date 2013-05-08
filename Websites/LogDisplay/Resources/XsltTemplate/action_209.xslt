<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:param name="IsInternal" select="1"></xsl:param>
    <xsl:output method="html"/>

  <xsl:template match="acc_customerupdsuper_commissionhr">
    <xsl:choose>
      <xsl:when test="@v151discount_31">
        <table class="tblDesc">
          <tr>
            <td class="colW1">{lbl:v151discount_31}</td>
            <td class="colW2">
              <xsl:value-of select="@v151discount_31" />
            </td>
            <td class="colW1">{lbl:v151discount_41}</td>
            <td class="colW2">
              <xsl:value-of select="@v151discount_41" />
            </td>
            <td class="colW1">{lbl:v151discount_51}</td>
            <td class="colW2">
              <xsl:value-of select="@v151discount_51" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:v152discount_41}</td>
            <td class="colW2">
              <xsl:value-of select="@v152discount_41" />
            </td>
            <td class="colW1">{lbl:v152discount_51}</td>
            <td class="colW2">
              <xsl:value-of select="@v152discount_51" />
            </td>
            <td class="colW1">{lbl:v153discount_51}</td>
            <td class="colW2">
              <xsl:value-of select="@v153discount_51" />
            </td>
          </tr>
          
          <tr>
            <td class="colW1">{lbl:ip}</td>
            <td class="colW2">
              <xsl:value-of select="@ip" />
            </td>
            <td class="colW1">{lbl:errcode}</td>
            <td class="colW2">
              <xsl:value-of select="@errcode" />
            </td>
            <td class="colW1">{lbl:errmsg}</td>
            <td class="colW2">
              <xsl:value-of select="@errmsg" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:language}</td>
            <td class="colW2">
              <xsl:value-of select="@language" />
            </td>
            <td class="colW1"></td>
            <td class="colW2">
            </td>
            <td class="colW1"></td>
            <td class="colW2">
            </td>
          </tr>
        </table>
      </xsl:when>
      <xsl:otherwise>
        <table class="tblDesc">
          <tr>
            <td class="colW1">{lbl:ukcomm}</td>
            <td class="colW2">
              <xsl:value-of select="@ukcomm" />
            </td>

            <td class="colW1">{lbl:othercomm}</td>
            <td class="colW2">
              <xsl:value-of select="@othercomm" />
            </td>
            <td class="colW1">{lbl:ip}</td>
            <td class="colW2">
              <xsl:value-of select="@ip" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:language}</td>
            <td class="colW2">
              <xsl:value-of select="@language" />
            </td>


            <td class="colW1">{lbl:errcode}</td>
            <td class="colW2">
              <xsl:value-of select="@errcode" />
            </td>
            <td class="colW1">{lbl:errmsg}</td>
            <td class="colW2">
              <xsl:value-of select="@errmsg" />
            </td>
          </tr>
        </table>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

</xsl:stylesheet>
