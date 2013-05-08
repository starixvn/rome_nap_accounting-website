<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:param name="IsInternal" select="1"></xsl:param>
    <xsl:output method="html"/>

  <xsl:template match="acc_customerupdagent_commissionhr">
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
            <td class="colW1">{lbl:subaccname}</td>
            <td class="colW2">
              <xsl:value-of select="@subaccname" />
            </td>
            <td class="colW1">{lbl:ip}</td>
            <td class="colW2">
              <xsl:value-of select="@ip" />
            </td>
            <td class="colW1">{lbl:errcode}</td>
            <td class="colW2">
              <xsl:value-of select="@errcode" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:errmsg}</td>
            <td class="colW2">
              <xsl:value-of select="@errmsg" />
            </td>
            <td class="colW1">{lbl:language}</td>
            <td class="colW2">
              <xsl:value-of select="@language" />
            </td>
            <td class="colTitle"></td>
            <td class="colValue">
            </td>
          </tr>
        </table>
      </xsl:when>
      <xsl:otherwise>
        <table class="tblDesc">
          <tr>

            <td class="colW1">{lbl:UKComm}</td>
            <td class="colW2">
              <xsl:value-of select="@ukcomm" />
            </td>

            <td class="colW1">{lbl:OtherComm}</td>
            <td class="colW2">
              <xsl:value-of select="@othercomm" />
            </td>
            <td class="colW1">{lbl:subaccname}</td>
            <td class="colW2">
              <xsl:value-of select="@subaccname" />
            </td>


          </tr>

          <tr>
            <td class="colW1">{lbl:IP}</td>
            <td class="colW2">
              <xsl:value-of select="@ip" />
            </td>
            <td class="colW1">{lbl:ErrCode}</td>
            <td class="colW2">
              <xsl:value-of select="@errcode" />
            </td>
            <td class="colW1">{lbl:ErrMsg}</td>
            <td class="colW2">
              <xsl:value-of select="@errmsg" />
            </td>

          </tr>
          <xsl:choose>
            <xsl:when test="$IsInternal = 0">

            </xsl:when>
            <xsl:otherwise>
              <tr>
                <td class="colW1">{lbl:SubAccID}</td>
                <td class="colW2">
                  <xsl:value-of select="@subaccid" />
                </td>
                <td class="colW1"></td>
                <td class="colW2">

                </td>
                <td class="colW1"></td>
                <td class="colW2">

                </td>
              </tr>
            </xsl:otherwise>
          </xsl:choose>
        </table>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

</xsl:stylesheet>
