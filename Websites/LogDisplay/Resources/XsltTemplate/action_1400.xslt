<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:param name="IsInternal" select="1"></xsl:param>
    <xsl:output method="html"/>

    <xsl:template match="acc_bettransdelspecial">
        <table class="tblDesc">
            <tr>
                <td class="colW1">
                    {lbl:transID}
                </td>
                <td class="colW2">
                    <xsl:value-of select="@transid" />
                </td>
              <td class="colW1">
                {lbl:matchID}
              </td>
              <td class="colW2">
                <xsl:value-of select="@matchid" />
              </td>

              <td class="colW1">
                {lbl:delBy}
              </td>
              <td class="colW2">
                <xsl:value-of select="@delby" />
              </td>

            </tr>

            <tr>
                <td class="colW1">
                    {lbl:ip}
                </td>
                <td class="colW2">
                    <xsl:value-of select="@ip" />
                </td>
                <td class="colW1">
                  {lbl:errCode}
                </td>
                <td class="colW2">
                  <xsl:value-of select="@errcode" />
                </td>
                <td class="colW1">
                  {lbl:errMsg}
                </td>
            </tr>

        </table>
    </xsl:template>
</xsl:stylesheet>
