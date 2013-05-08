<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:param name="IsInternal" select="1"></xsl:param>
    <xsl:output method="html"/>

    <xsl:template match="dot_dep_customerinsmember_ag">
        <table class="tblDesc">
            <tr>
                <!--<td class="colW1">{lbl:custid}</td>
                <td class="colW2">
                    <xsl:value-of select="@custid" />
                </td>-->
                <td class="colW1">{lbl:MinBet}</td>
                <td class="colW2">
                    <xsl:value-of select="@minbet" />
                </td>
                <td class="colW1">{lbl:MaxBet}</td>
                <td class="colW2">
                    <xsl:value-of select="@maxbet" />
                </td>
                <td class="colW1">{lbl:MaxPerMatch}</td>
                <td class="colW2">
                    <xsl:value-of select="@maxpermatch" />
                </td>
            </tr>
            
        </table>
    </xsl:template>


</xsl:stylesheet>
