<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:param name="IsInternal" select="1"></xsl:param>
    <xsl:output method="html"/>

    <xsl:template match="acc_virtualratemulupd_sum">
        <table class="tblDesc">
            <tr>
                <td class="colW1">{lbl:TotalUser}</td>
                <td class="colW2">
                    <xsl:value-of select="@totaluser" />
                </td>
                <td class="colW1">{lbl:IP}</td>
                <td class="colW2">
                    <xsl:value-of select="@ip" />
                </td>

                <td class="colW1">{lbl:FileName}</td>
                <td class="colW2">
                    <xsl:value-of select="@filename" />
                </td>
            </tr>
            <tr>
                <td class="colW1">{lbl:Size}</td>
                <td class="colW2">
                    <xsl:value-of select="@size" />
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
