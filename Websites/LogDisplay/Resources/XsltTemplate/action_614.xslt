<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:param name="IsInternal" select="1"></xsl:param>
    <xsl:output method="html"/>

    <xsl:template match="acc_customerupdmovesite">
        <table class="tblDesc">
            <tr>
                <td class="colW1">{lbl:oldSite}</td>
                <td class="colW2">
                    <xsl:value-of select="@oldsite" />
                </td>
                <td class="colW1">{lbl:newSite}</td>
                <td class="colW2">
                  <xsl:value-of select="@newsite" />
                </td>
                <td class="colW1">{lbl:ip}</td>
                <td class="colW2">
                  <xsl:value-of select="@ip" />
                </td>
            </tr>
        </table>

    </xsl:template>
</xsl:stylesheet>
