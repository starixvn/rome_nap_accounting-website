<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:param name="IsInternal" select="1"></xsl:param>
    <xsl:output method="html"/>

    <xsl:template match="acc_virtualratemulupd">
        <table class="tblDesc">
            <tr>
                <td class="colW1">{lbl:Rate}</td>
                <td class="colW2">
                    <xsl:value-of select="@rate" />
                </td>
                <td class="colW1">{lbl:UserName}</td>
                <td class="colW2">
                    <xsl:value-of select="@username" />
                </td>
                <td class="colW1">{lbl:IP}</td>
                <td class="colW2">
                    <xsl:value-of select="@ip" />
                </td>
            </tr>
            <tr>
                <td class="colW1">{lbl:FileName}</td>
                <td class="colW2">
                    <xsl:value-of select="@filename" />
                </td>
              <xsl:choose>
                <xsl:when test="@size">
                  <td class="colW1">{lbl:Size}</td>
                  <td class="colW2">
                    <xsl:value-of select="@size" />
                  </td>
                </xsl:when>
                <xsl:otherwise>
                  <td class="colW1">{lbl:FileSize}</td>
                  <td class="colW2">
                    <xsl:value-of select="@filesize" />
                  </td>
                </xsl:otherwise>
              </xsl:choose>

              <td class="colW1"></td>
                <td class="colW2">
                    
                </td>
            </tr>
        </table>

    </xsl:template>
</xsl:stylesheet>
