<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:param name="IsInternal" select="1"></xsl:param>
    <xsl:output method="html"/>

    <xsl:template match="acc_customerupdsuper_minmaxbethr">
        <table class="tblDesc">
            <tr>
                <td class="colW1">{lbl:MinBHR}</td>
                <td class="colW2">
                    <xsl:value-of select="@minb" />
                </td>
                <td class="colW1">{lbl:maxbHR}</td>
                <td class="colW2">
                    <xsl:value-of select="@maxb" />
                </td>
                <td class="colW1">{lbl:MPMv151}</td>
                <td class="colW2">
                    <xsl:value-of select="@mpm" />
                </td>
            </tr>
            
            <tr>
                <td class="colW1">{lbl:Language}</td>
                <td class="colW2">
                    <xsl:value-of select="@language" />
                </td>
                <td class="colW1">{lbl:IP}</td>
                <td class="colW2">
                    <xsl:value-of select="@ip" />
                </td>
                <td class="colW1">{lbl:ErrCode}</td>
                <td class="colW2">
                    <xsl:value-of select="@errcode" />
                </td>
            </tr>
            <tr>
                <td class="colW1">{lbl:ErrMsg}</td>
                <td class="colW2">
                    <xsl:value-of select="@errmsg" />
                </td>
                <td class="colTitle"></td>
                <td class="colValue">

                </td>
                <td class="colTitle"></td>
                <td class="colValue">

                </td>
            </tr>
        </table>
    </xsl:template>



</xsl:stylesheet>
