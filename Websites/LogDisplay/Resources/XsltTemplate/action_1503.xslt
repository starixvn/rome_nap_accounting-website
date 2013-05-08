<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:param name="IsInternal" select="1"></xsl:param>
    <xsl:output method="html"/>

    <xsl:template match="dba_archiveaccount1_rollback">
        <table class="tblDesc">
            <tr>
                <td class="colW1">
                    {lbl:AccName}
                </td>
                <td class="colW2">
                    <xsl:value-of select="@accname" />
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
                <td class="colW2">
                    <xsl:value-of select="@errmsg" />
                </td>
            </tr>

        </table>
    </xsl:template>
</xsl:stylesheet>
