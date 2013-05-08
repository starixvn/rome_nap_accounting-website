<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:param name="IsInternal" select="1"></xsl:param>
    <xsl:output method="html"/>

    <xsl:template match="acc_customerupdusergroup">
        <table class="tblDesc">
            <tr>
              <xsl:choose>
                <xsl:when test="@custname">
                  <td class="colW1">{lbl:CustName}</td>
                  <td class="colW2">
                    <xsl:value-of select="@custname" />
                  </td>
                </xsl:when>
                <xsl:when test="@username">
                  <td class="colW1">{lbl:UserName}</td>
                  <td class="colW2">
                    <xsl:value-of select="@username" />
                  </td>
                </xsl:when>
              </xsl:choose>
              
              <xsl:choose>
                <xsl:when test="@from">
                  <td class="colW1">{lbl:From}</td>
                  <td class="colW2">
                    <xsl:value-of select="@from" />
                  </td>
                </xsl:when>
                <xsl:when test="@ugroup">
                  <td class="colW1">{lbl:Ugroup}</td>
                  <td class="colW2">
                    <xsl:value-of select="@ugroup" />
                  </td>
                </xsl:when>
              </xsl:choose>
              
              <xsl:choose>
                <xsl:when test="@to">
                  <td class="colW1">{lbl:To}</td>
                  <td class="colW2">
                    <xsl:value-of select="@to" />
                  </td>
                </xsl:when>
                <xsl:otherwise>
                  <td class="colW1"></td>
                  <td class="colW2">
                  </td>
                </xsl:otherwise>
              </xsl:choose>
            </tr>
            <tr>
                <td class="colW1">{lbl:errCode}</td>
                <td class="colW2">
                    <xsl:value-of select="@errcode" />
                </td>
                <td class="colW1">{lbl:errMsg}</td>
                <td class="colW2">
                    <xsl:value-of select="@errmsg" />
                </td>
              <xsl:choose>
                <xsl:when test="@ip">
                  <td class="colW1">{lbl:IP}</td>
                  <td class="colW2">
                    <xsl:value-of select="@ip" />
                  </td>
                </xsl:when>
                <xsl:when test="@returnerrcode">
                  <td class="colW1">{lbl:ReturnErrCode}</td>
                  <td class="colW2">
                    <xsl:value-of select="@returnerrcode" />
                  </td>
                </xsl:when>
                <xsl:otherwise>
                  <td class="colW1"></td>
                  <td class="colW2">
                  </td>
                </xsl:otherwise>
              </xsl:choose>
            </tr>
        </table>



    </xsl:template>
</xsl:stylesheet>
