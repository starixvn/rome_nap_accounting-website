<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:param name="IsInternal" select="1"></xsl:param>
    <xsl:output method="html"/>

    <xsl:template match="age_customerupdpwd">
        <table class="tblDesc">
            <tr>
                <!--<td class="colW1">{lbl:UserID}</td>
        <td class="colW2">
          <xsl:value-of select="@userid" />
        </td>-->

                <td class="colW1">{lbl:SubAccount}</td>
                <td class="colW2">
                    <xsl:value-of select="@subaccount" />
                </td>
                <td class="colW1">{lbl:IP}</td>
                <td class="colW2">
                    <xsl:value-of select="@ip" />
                </td>
                <td class="colW1"></td>
                <td class="colW2">
                    
                </td>
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
