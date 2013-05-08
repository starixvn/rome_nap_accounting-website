<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:param name="IsInternal" select="1"></xsl:param>
    <xsl:output method="html"/>

    <xsl:template match="acc_updcsstatus">
        <table class="tblDesc">
            <tr>
                <td class="colW1">
                    {lbl:disabledCS}
                </td>
                <td class="colW2">
                    <xsl:value-of select="@disabledcs" />
                </td>
                <td class="colW1">
                    {lbl:syncCS}
                </td>
                <td class="colW2">
                    <xsl:value-of select="@synccs" />
                </td>
                <td class="colW1">
                  {lbl:enableDownline}
                </td>
                <td class="colW2">
                  <xsl:value-of select="@enabledownline" />
                </td>
            </tr>

            <tr>
                <td class="colW1">
                    {lbl:status}
                </td>
                <td class="colW2">
                    <xsl:value-of select="@status" />
                </td>
                <td class="colW1">
                    {lbl:errMsg}
                </td>
                <td class="colW2">
                    <xsl:value-of select="@errmsg" />
                </td>
                <td class="colW1">
                  {lbl:subAccName}
                </td>
                <td class="colW2">
                  <xsl:value-of select="@subaccname" />
                </td>
            </tr>

            <tr>
                <td class="colW1">
                    {lbl:language}
                </td>
                <td class="colW2">
                    <xsl:value-of select="@language" />
                </td>
                <td class="colW1">
                  {lbl:loginIP}
                </td>
                <td class="colW2">
                  <xsl:value-of select="@loginip" />
                </td>
                <td class="colW1"></td>
                <td class="colW2">

                </td>
            </tr>
        </table>
    </xsl:template>
</xsl:stylesheet>
