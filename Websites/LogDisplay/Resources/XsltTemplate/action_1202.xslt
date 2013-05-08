<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:param name="IsInternal" select="1"></xsl:param>
    <xsl:output method="html"/>

    <xsl:template match="acc_customerupdresetmembercredit">
        <table class="tblDesc">
            <tr>

                <td class="colW1">
                    {lbl:closedOnly}
                </td>
                <td class="colW2">
                    <xsl:value-of select="@closedonly" />
                </td>
                <td class="colW1">
                    {lbl:errCode}
                </td>
                <td class="colW2">
                    <xsl:value-of select="@errcode" />
                </td>

                <!--<td class="colW1">
                    {lbl:agentID}
                </td>
                <td class="colW2">
                    <xsl:value-of select="@agentid" />
                </td>-->
                <!--<td class="colW1">
                    {lbl:userID}
                </td>
                <td class="colW2">
                    <xsl:value-of select="@userid" />
                </td>-->
                <xsl:choose>
                    <xsl:when test="$IsInternal = 0">
                        <td class="colW1"></td>
                        <td class="colW2">

                        </td>
                    </xsl:when>
                    <xsl:otherwise>
                        <td class="colW1">{lbl:agentID}</td>
                        <td class="colW2">

                            <xsl:value-of select="@agentid" />
                        </td>

                    </xsl:otherwise>
                </xsl:choose>
            </tr>

            <tr>

                <td class="colW1">
                    {lbl:errMsg}
                </td>
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
