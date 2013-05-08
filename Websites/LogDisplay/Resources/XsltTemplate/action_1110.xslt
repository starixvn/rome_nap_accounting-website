<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:param name="IsInternal" select="1"></xsl:param>
    <xsl:output method="html"/>

    <xsl:template match="acc_customerupdautosuspend">
        <table class="tblDesc">
            <tr>
              <xsl:choose>
                <xsl:when test="@autosuspend">
                  <td class="colW1">
                    {lbl:autoSuspend}
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@autosuspend" />
                  </td>  
                </xsl:when>
                <xsl:when test="@asuspend">
                  <td class="colW1">
                    {lbl:autoSuspend}
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@asuspend" />
                  </td>
                </xsl:when>
              </xsl:choose>
                
                <td class="colW1">
                  {lbl:errCode}
                </td>
                <td class="colW2">
                  <xsl:value-of select="@errcode" />
                </td>
                <td class="colW1">
                  {lbl:errMsg}
                </td>
                <td class="colW2">
                  <xsl:value-of select="@errmsg" />
                </td>
            </tr>

            <tr>
                <td class="colW1">
                    {lbl:ip}
                </td>
                <td class="colW2">
                    <xsl:value-of select="@ip" />
                </td>
                <td class="colW1"></td>
                <td class="colW2">

                </td>
                <td class="colW1"></td>
                <td class="colW2">

                </td>
            </tr>
        </table>
    </xsl:template>
</xsl:stylesheet>
