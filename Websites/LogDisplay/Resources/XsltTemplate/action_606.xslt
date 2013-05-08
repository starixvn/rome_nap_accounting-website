<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:param name="IsInternal" select="1"></xsl:param>
    <xsl:output method="html"/>

    <xsl:template match="acc_customerupdmoveagent">
        <table class="tblDesc">
            <tr>
                <td class="colW1">{lbl:agentName}</td>
                <td class="colW2">
                    <xsl:value-of select="@agentname" />
                </td>

                <td class="colW1">{lbl:newMasterName}</td>
                <td class="colW2">
                    <xsl:value-of select="@newmastername" />
                </td>
                <td class="colW1">{lbl:ip}</td>
                <td class="colW2">
                    <xsl:value-of select="@ip" />
                </td>
            </tr>
            <tr>
                <!--<td class="colW1">{lbl:oldMasterId}</td>
                <td class="colW2">
                    <xsl:value-of select="@oldmasterid" />
                </td>
                <td class="colW1">{lbl:newMasterId}</td>
                <td class="colW2">
                    <xsl:value-of select="@newmasterid" />
                </td>-->
                <xsl:choose>
                    <xsl:when test="$IsInternal = 0">
                        <td class="colW1"></td>
                        <td class="colW2">

                        </td>
                    </xsl:when>
                    <xsl:otherwise>
                        <td class="colW1">{lbl:oldMasterId}</td>
                        <td class="colW2">

                            <xsl:value-of select="@oldmasterid" />
                        </td>

                    </xsl:otherwise>
                </xsl:choose>
                <xsl:choose>
                    <xsl:when test="$IsInternal = 0">
                        <td class="colW1"></td>
                        <td class="colW2">

                        </td>
                    </xsl:when>
                    <xsl:otherwise>
                        <td class="colW1">{lbl:newMasterId}</td>
                        <td class="colW2">

                            <xsl:value-of select="@newmasterid" />
                        </td>

                    </xsl:otherwise>
                </xsl:choose>

                <td class="colW1"></td>
                <td class="colW2">

                </td>
                <!--<td class="colW1">{lbl:userID}</td>
        <td class="colW2">
          <xsl:value-of select="@userid" />
        </td>-->
                <!--<xsl:choose>
                    <xsl:when test="$IsInternal = 0">
                        <td class="colW1"></td>
                        <td class="colW2">

                        </td>
                    </xsl:when>
                    <xsl:otherwise>
                        <td class="colW1">{lbl:userID}</td>
                        <td class="colW2">

                            <xsl:value-of select="@userid" />
                        </td>

                    </xsl:otherwise>
                </xsl:choose>-->

            </tr>
        </table>

    </xsl:template>
</xsl:stylesheet>
